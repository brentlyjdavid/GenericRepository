# Generic Repository

This is a library that helps you elminiate simple mistakes by taking care of handling updating fields, applying filtering for certain scenarios during query and providing a unified API for accessing the database.

### Interfaces for Tables

The vast majority of the interfaces for the tables/entities start with `ISaveable`.  The exception is `IOrderable` and `IOrderableDescending`.  Below is a list of the possible interfaces you can add to dictate behavior changes during query or save.

* `ISaveable<TKey>` (Used to dictate PK)
* `ISaveableActive` (IsActive)
* `ISaveableCreate` (DateCreatedUtc)
* `ISaveableDelete` (Nullable DateDeletedUtc)
* `ISaveableEnd` (EndUtc)
* `ISaveableStart` (StartUtc)
* `ISaveableTracking` (Nullable DateLastModifiedUtc)
* `ISaveableUserCreateTracking<TUserKey>` (CreatedById)
* `ISaveableUserUpdateTracking<TUserKey>` (LastModifiedById)
* `ISaveableUserTracking<TUserKey>` (Just a parent interface with the last two, with strings as Ids)
* `IEntityView` (Used in more advanced features of the repository, mainly useful for situations where you have other contexts you are querying for)


### Behaviors of Interfaces

##### ISaveableActive & ISaveableDelete

These two in specific combinations apply different behaviors to your queries.  For example, an entity with only `ISaveableActive` will *only* return entities that have IsActive = 1.  An entity with `ISaveableActive` and `ISaveableDelete` will return all entities with IsActive true or false, but not return any that have a value in the DateDeletedUtc Field.

#### ISaveableDelete

This interface, in addition to the above query behaviors, modifies the saving logic on an entity.  If this interface exists, the repository will perform a 'Soft Delete', meaning it will set a field called DateDeletedUtc to the current date.  If this interface does not exist, it will attempt to fully remove it from the database (and fail if there are foreign key constraint issues)

#### ISaveableCreate

As it sounds, the repository will add a DateCreatedUtc value, provided you didn't already give the entity one prior to saving

#### ISaveableTracking

This interface tracks the last updated date, and will always update the date on every save except for the first intial add to the database.

#### ISaveableUserCreateTracking<TUserKey>, ISaveableUserUpdateTracking<TUserKey> & ISaveableUserTracking<TUserKey>

These are a little bit tricky and they require the use of the built in functionality provided to you in .NET.  On every save or update, it will go and check to see if the claims principle is filled out and authenticated.  If it is, it will put the current logged in User Id into that field.  Default implementation is to check Microsofts Schema for NameIdentifier and fallback to SubjectId.

#### ISaveable<TKey>

This gives a field name of Id in the type you chose.


## Examples

    public class BlogPost : ISaveable<string>, ISaveableCreate, ISaveableDelete, ISaveableUserTracking, ISaveableTracking 
    {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public string CreatedById { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime? DateLastModifiedUtc { get; set; }
    }

## Top Level Repository

One of the benefits of this library is the container class used to hold all the repositories.  They all share one context, therefore it's very simple to ensure that if you are writing thing that requires a multi step process, it's easy to wrap that into transactions if needed.

### Interfaces for Table Repositories & Default Implementations

* `IRepository<TKey, TEntity, TUserKey>`
* `IRepositoryCompositKeys<TEntity, TUserKey>`
* `IView<TEntity>`

### Overrides

A huge benefit of these repositories is the default behaviors that go with them.  However, inevitably you will want to override them or at add to their default functions.  There are three methods you can choose from.

* `ApplyAdditionalConstraints`
    * This gives you the current IQueryable being used and allows you to append to it, and return the changed query
* `ApplyAdditionalHangfireOnlyConstraints`
    * Same drill, gives you an IQueryable, return the changed query.  This is only called if IsHangfire is true when the IRepository was created
* `ApplyAdditionalUserClaimsOnlyConstraint`
    * Same drill, gives you an IQueryable, return the changed query.  This is only called if the user is authenticated. 

### Examples

A property such as 

`public IRepository<string, TestClass, string> TestClasses { get; set; }`  

would get instantiated with 

`TestClasses = new Repository<string, TestClass, string>(Context, User, IsHangfire)`

Hangfire is an overload to tell the repository to ignore user items, since Hangfire runs in the background and doesn't have a logged in user or claims to look at.

If you had a multi-tenant environment and needed to make sure that all your queries ALWAYS took into account the company they belong to, but still leave your regular codebase quite clean.  Your repository initilalization might look like this:

    TimeCards = new Repository<string, TimeCard, string>(Context, User, IsHangfire)
    {
        ApplyAdditionalUserClaimsOnlyConstraints = (timeCards, user) =>
        {
            var companyId = user.GetClaimValue(AppClaimTypes.CompanyId);
            return timeCards.Where(m => m.PayPeriod.CompanyId == companyId);
        }
    };
     
So your Query in your project overall would still be `TimeCards.ToList()` but instead of this returning the entire table, it only returns the current company's timecards.

## Container Repository for all others

To make things easier on dependency injection, there is a base class you can use to hold all of your properties with `IRepository<...>`

### Example of a ASP.NET Core Project

    public class AppRepository : ApplicationRepositoryBase<AppDbContext>
    {
        public IRepository<string, TestClass, string> TestClasses { get; set; }
        public IRepositoryCompositKeys<TestComposite, string> TestComposites { get; set; }

        public IView<TestClassView> TestClassesView { get; set; }
        public AppRepository(AppDbContext ctx, IHttpContextAccessor ctxAccessor, bool isHangfire = false) : base(ctx, ctxAccessor.HttpContext?.User, isHangfire)
        {
        }

        public override void Initialize()
        {
            TestClasses = new Repository<string, TestClass, string>(Context, User, IsHangfire);
            TestComposites = new Repository<TestComposite, string>(Context, User, IsHangfire);
            TestClassesView = new View<TestClassView>(Context, User, IsHangfire);
        }
    }

### Usage examples

Using the above as the conatiner, lets imagine we have injected this into our class as a read only property called `_repo`

In order to add or upate into the Test Classes table, we would do the following

`_repo.TestClasses.AddOrUpdateAndSave(testClassInstance);`

### Queries

`_repo.TestClasses.GetById(id);`
`_repo.Testclasses.Where(m=>m.Name == "Testing").ToList();`
`_repo.TestClasses.AsQueryable()` (applies all filtering but just returns a queryable, useful for paging entire tables)
`_repo.TestClasses.DeleteAndSave(id)` (adds date deleted to field if deleted interface exists, otherwise attempts to do a real delete from the database)

All queries have Async versions and the syntax follows very closely to Entity Framework.
