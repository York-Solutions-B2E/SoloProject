namespace WebApi.Controllers
{
    public class Subscription
    {
        // Placeholder subscription
        [Subscribe]
        public string DummySubscription([EventMessage] string message) => message;
    
    }
    
}