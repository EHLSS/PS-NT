using BWA.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Load AD configuration for authentication
IConfiguration ServerConfiguration;
ServerConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
ConfigureLdap();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


void ConfigureLdap()
{
    string path = ServerConfiguration.GetValue<string>("Ldap:Path");
    string usernameTemplate = ServerConfiguration.GetValue<string>("Ldap:UsernameTemplate");
    string filterAttribute = ServerConfiguration.GetValue<string>("Ldap:FilterAttribute");
    string firstNameAttribute = ServerConfiguration.GetValue<string>("Ldap:FirstNameAttribute");
    string lastNameAttribute = ServerConfiguration.GetValue<string>("Ldap:LastNameAttribute");
    string externalIdAttribute = ServerConfiguration.GetValue<string>("Ldap:ExternalIdAttribute");
    string emailAttribute = ServerConfiguration.GetValue<string>("Ldap:EmailAttribute");
    string authenticationType = ServerConfiguration.GetValue<string>("Ldap:AuthenticationType");
    string serviceAccountUsername = ServerConfiguration.GetValue<string>("Ldap:ServiceAccountUsername");

    LdapConfig ldapConfig = new();  // LoginController._LdapConfig;
    ldapConfig.Path = path;
    ldapConfig.UsernameTemplate = usernameTemplate;
    ldapConfig.FilterAttribute = filterAttribute;
    ldapConfig.FirstNameAttribute = firstNameAttribute;
    ldapConfig.LastNameAttribute = lastNameAttribute;
    ldapConfig.ExternalIdAttribute = externalIdAttribute;
    ldapConfig.EmailAttribute = emailAttribute;
    ldapConfig.AuthenticationType = authenticationType;
    ldapConfig.ServiceAccountUsername = serviceAccountUsername;
}

public class LdapConfig
{
    public string Path { get; set; } = "";
    public string UsernameTemplate { get; set; } = "{username}";
    public string FilterAttribute { get; set; } = "userprincipalname";
    public string FirstNameAttribute { get; set; } = "givenname";
    public string LastNameAttribute { get; set; } = "sn";
    public string EmailAttribute { get; set; } = "mail";
    public string ExternalIdAttribute { get; set; } = "userprincipalname";
    public string AuthenticationType { get; set; } = "None";
    public string ServiceAccountUsername { get; set; } = "srvc_rcl_mvg1";
}
