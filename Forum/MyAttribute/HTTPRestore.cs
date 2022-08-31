namespace Forum.MyAttribute
{
    public class HTTPRestore : Attribute
    {
        private readonly HttpContextAccessor _accessor;

        public HTTPRestore(HttpContextAccessor accessor)
        {
            _accessor = accessor;
            //var myValue = _accessor.HttpContext.Request.RouteValues.GetValueOrDefault["myValue"];
            // ...
        }
    }
}
