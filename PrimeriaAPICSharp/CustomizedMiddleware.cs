namespace PrimeriaAPICSharp
{
    public class CustomizedMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey("UsuarioLogado"))
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("O header UsuarioLogado deve ser preenchido");
                    return;
                }

                if(httpContext.Request.Headers.TryGetValue("UsuarioLogado", out var value) && value != "Admin")
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Acesso não autorizado.");
                    return;
                }
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync($"Ocorreu algum erro: {ex.Message}");
                return;
            }
           
        }
    }
}
