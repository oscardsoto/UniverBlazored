namespace UniverBlazored.Tests;

public sealed class UniverUserManagerTests
{
    private readonly FakeUniverJsInterop fake = new();
    private readonly UniverUserManager manager;

    public UniverUserManagerTests()
    {
        manager = new(fake, "test-instance");
    }

    [Fact]
    public async Task GetCurrentUser_creates_correct_queue()
    {
        await manager.GetCurrentUser();

        var action = fake.Actions.Single();
        Assert.Equal("test-instance", action.InstanceId);
        Assert.Equal(RecordedActionKind.ResolveContextQueueT, action.Kind);
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item => Assert.Equal("getCurrentUser", item.methodName));
    }

    [Fact]
    public async Task ListAllUsers_creates_correct_queue()
    {
        await manager.ListAllUsers();

        var action = fake.Actions.Single();
        Assert.Equal("test-instance", action.InstanceId);
        Assert.Equal(RecordedActionKind.ResolveContextQueueT, action.Kind);
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item => Assert.Equal("list", item.methodName));
    }

    [Fact]
    public async Task GetUser_creates_correct_queue()
    {
        await manager.GetUser("user-42");

        var action = fake.Actions.Single();
        Assert.Equal("test-instance", action.InstanceId);
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item =>
            {
                Assert.Equal("getUser", item.methodName);
                Assert.Equal("user-42", item.args[0]);
            });
    }

    [Fact]
    public async Task AddUser_single_creates_correct_queue()
    {
        var user = new UniverUser { userID = "u1", name = "Alice" };

        await manager.AddUser(user, setCurrent: false);

        var action = fake.Actions.Single();
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item =>
            {
                Assert.Equal("addUser", item.methodName);
                var arg = Assert.IsType<UniverUser>(item.args[0]);
                Assert.Equal("u1", arg.userID);
            });
    }

    [Fact]
    public async Task AddUser_with_setCurrent_calls_add_then_setCurrent()
    {
        var user = new UniverUser { userID = "u2", name = "Bob" };

        await manager.AddUser(user, setCurrent: true);

        Assert.Equal(2, fake.Actions.Count);
        var addAction = fake.Actions[0];
        var setCurrentAction = fake.Actions[1];
        Assert.Equal("addUser", addAction.Queue.Last().methodName);
        Assert.Equal("setCurrentUser", setCurrentAction.Queue.Last().methodName);
    }

    [Fact]
    public async Task AddUser_params_creates_queue_per_user()
    {
        var userA = new UniverUser { userID = "u1" };
        var userB = new UniverUser { userID = "u2" };

        await manager.AddUser(userA, userB);

        Assert.Equal(2, fake.Actions.Count);
        Assert.Equal("u1", ((UniverUser)fake.Actions[0].Queue.Last().args[0]).userID);
        Assert.Equal("u2", ((UniverUser)fake.Actions[1].Queue.Last().args[0]).userID);
    }

    [Fact]
    public async Task CleanList_creates_correct_queue()
    {
        await manager.CleanList();

        var action = fake.Actions.Single();
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item => Assert.Equal("clear", item.methodName));
    }

    [Fact]
    public async Task DeleteUser_creates_correct_queue()
    {
        await manager.DeleteUser("user-99");

        var action = fake.Actions.Single();
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item =>
            {
                Assert.Equal("delete", item.methodName);
                Assert.Equal("user-99", item.args[0]);
            });
    }

    [Fact]
    public async Task SetCurrentUser_creates_correct_queue()
    {
        var user = new UniverUser { userID = "u3", name = "Charlie" };

        await manager.SetCurrentUser(user);

        var action = fake.Actions.Single();
        Assert.Collection(action.Queue,
            item => Assert.Equal("getUserService", item.methodName),
            item =>
            {
                Assert.Equal("setCurrentUser", item.methodName);
                var arg = Assert.IsType<UniverUser>(item.args[0]);
                Assert.Equal("u3", arg.userID);
            });
    }

    [Fact]
    public async Task uses_instanceId_from_constructor()
    {
        var custom = new UniverUserManager(fake, "my-instance");

        await custom.GetCurrentUser();

        var action = fake.Actions.Single();
        Assert.Equal("my-instance", action.InstanceId);
    }
}
