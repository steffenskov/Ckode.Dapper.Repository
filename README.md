# Ckode.Dapper.Repository
This is an extension library for the Dapper ORM, giving you simple-to-use repositories for all your database access code.

## Installation:

I recommend using the NuGet package: https://www.nuget.org/packages/Ckode.ServiceLocator/ however you can also simply clone the repository and compile the project yourself.

As the project is licensed under MIT you're free to use it for pretty much anything you want.

You also need to install Dapper yourself, again I'd recommend NuGet: https://www.nuget.org/packages/Dapper/

As for versioning of Dapper, you're actually (somewhat) free to choose whichever you want, as this library isn't built targetting a specific version of Dapper.

## Requirements:

The library requires .Net 5.0 with C# 9 or later, as it's using the "record" type rather than "class" for representing the single entities. (e.g. An "User" from your "Users" table)

Also it currently only supports MS Sql, but feel free to branch it and create support for MySql, PostGre or whatever you're using (as long as Dapper supports it, this library can too)

## Upcoming features:

- Repository for aggregates, like e.g. an Order entity with an IList<OrderLine> property.
- Built-in caching with automatic cache invalidation
- Fixing the compiler warning about non-nullable properties on the entity records. (only an issue in projects with nullable enabled, and it's just a "false positive" type of warning)

## Usage:

In order to avoid building this library for a specific Dapper version, I've added an injection point for injecting the necessary Dapper extension methods into the repositories.
To only do this once, I recommend you start by creating a class called DapperInjection:

    using SqlMapper = Dapper.SqlMapper;

    namespace YourNameSpaceHere
    {
        public class DapperInjection<TEntity> : IDapperInjection<TEntity>
        where TEntity : TableEntity
        {
            public static DapperInjection<TEntity> Instance { get; }

            static DapperInjection()
            {
                Instance = new DapperInjection<TEntity>();
            }

            private DapperInjection() // Singleton pattern
            {
            }

            public QuerySingleDelegate<TEntity> QuerySingle => SqlMapper.QuerySingle<TEntity>;

            public QuerySingleDelegate<TEntity> QuerySingleOrDefault => SqlMapper.QuerySingleOrDefault<TEntity>;

            public QueryDelegate<TEntity> Query => SqlMapper.Query<TEntity>;

            public QuerySingleAsyncDelegate<TEntity> QuerySingleAsync => SqlMapper.QuerySingleAsync<TEntity>;

            public QuerySingleAsyncDelegate<TEntity> QuerySingleOrDefaultAsync => SqlMapper.QuerySingleOrDefaultAsync<TEntity>;

            public QueryAsyncDelegate<TEntity> QueryAsync => SqlMapper.QueryAsync<TEntity>;
        }
    }

What's going on here is you're injecting delegates to the Dapper extension methods into the Repository library.

Secondly I'd recommend creating a sort of "base repository" class for your project, which handles creating an IDbConnection etc. It could look something like this:

    using System.Data;
    using System.Data.SqlClient;
    using Ckode.Dapper.Repository.Sql;

    namespace YourNameSpaceHere
    {
        public abstract class BasePrimaryKeyRepository<TPrimaryKeyEntity, TEntity> : PrimaryKeyRepository<TPrimaryKeyEntity, TEntity>
        where TPrimaryKeyEntity : TableEntity
        where TEntity : TPrimaryKeyEntity
        {
            protected override IDapperInjection<TEntity> DapperInjection => DapperInjection<TEntity>.Instance; // Using the Singleton we created above

            protected override IDbConnection CreateConnection()
            {
                return new SqlConnection("Your connection string"); // You probably shouldn't hardcode your connection strings, this is just an example
            }
        }
    }

The "base repository" we just created is for tables with a primary key. If you have any heap tables (tables without a primary key), you should create a similar "BaseHeapRepository" class inheriting HeapRepository.


That's the prerequisites taken care of, now onto actually using these classes. For this example we're going to create a very basic UserRepository mapping to a "Users" table looking like this:

    CREATE TABLE Users
    (
        UserID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        Username VARCHAR(50) NOT NULL,
        Password VARCHAR(50) NOT NULL, // Don't store passwords in plain text please, this is just for illustration purposes
        Description VARCHAR(MAX) NULL,
        DateCreated DATETIME() NOT NULL DEFAULT(GETDATE())
    )


Now in order to have some nice method overloads, we're actually splitting the entity class into two, using inheritance along the way. We're also using the type "record" instead of "class". This is because it simplifies making entities immutable and creating new instances with the changes you want. Immutability allows for some very aggressive memory caching, because the entities become (at least somewhat) thread-safe.

Our UserEntity.cs file would therefore look like this:

    using System;
    using Ckode.Dapper.Repository.Attributes;

    namespace YourNameSpaceHere
    {
        public record UserPrimaryKeyEntity : TableEntity
        {
            [PrimaryKeyColumn(isIdentity: true)]
            public int Id { get; init; }
        }

        public record UserEntity : UserPrimaryKeyEntity
        {
            [Column]
            public string Username { get; init; }

            [Column]
            public string Password { get; init; }

            [Column]
            public string? Description { get; init; }

            [Column(hasDefaultConstraint: true)] // Marked with "hasDefaultConstraint", as the table does indeed have DEFAULT(GETDATE()) on this column
            public DateTime DateCreated { get; } // No init as I want this property completely read-only in .Net, it's only ever set once by the SQL server
        }
    }

You'll notice I've enabled nullable in this case, and I'm marking Description as string?. If you're not working with nullable just remove the ?.
Also this example WILL currently generate compiler warnings about the properties Username, Password and DateCreated because they're not nullable and no constructor ensures they have a value. I'm currently working on figuring out how to remove these.

Why the split between two record types you ask? It's for simplifying the Delete and Get methods on the repository.
Rather than having to supply a full UserEntity instance as parameter to them, you can now just supply the UserPrimaryKeyEntity (e.g. the Id of the user to Delete or Get)

The final part is defining your repository, this in turn requires very little code as most of the functionality is built-in:

    namespace YourNameSpaceHere
    {
        public class UserRepository : BasePrimaryKeyRepository<UserPrimaryKeyEntity, UserEntity>
        {
            protected override string TableName => "Users";
        }
    }

Quite easy that one huh? :-)

The UserRepository now gives you access to the following built-in methods:
- UserEntity Delete(UserPrimaryKeyEntity entity)
- UserEntity Insert(UserEntity entity)
- UserEntity Get(UserPrimaryKeyEntity entity)
- IEnumerable<UserEntity> GetAll()
- UserEntity Update(UserEntity entity)

All of which have an Async variant as well. You'll notice all methods, including Delete, return an instance of UserEntity. This is because whatever data you've just manipulated using Delete, Insert or Update is returned to you as an instance.
In our case this means the result from Insert will actually contain the DateCreated value the database generated itself.
Furthermore you're getting all the properties from the record you've just deleted, which can be quite handy for cache invalidation.

Should you want to add custom queries to your repository, it has a bunch of the Dapper extensions built-in for you to call, they create a connection themselves so it's as simple as:

    public IEnumerable<UserEntity> GetUsersWithoutDescription()
    {
        return Query($"SELECT * FROM {FormattedTableName} WHERE Description IS NULL");
    }

Notice the {FormattedTableName} there, that's a property which contains the proper Schema and TableName correctly formatted. By using this rather than typing out the name yourself, it'll be easier for you if you ever rename the table or move it to a different Schema.


One finally notice: I'd highly recommend checking out the different parameters you can assign to [Column] and [PrimaryKey], as well as checking out what properties can be overridden in your BasePrimaryKeyRepository and which methods it already has for you to use. This should give you further insight into how the library works.