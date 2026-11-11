using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.DataProtection;
using LojaCupcakes.Data;
using LojaCupcakes.Services;
using LojaCupcakes.Tests;

// =====================================================================
// RESOLUÇÃO EXPLÍCITA DA DLL NATIVA DO SQLITE (0x800700CE - MAX_PATH)
// =====================================================================
IntPtr ResolveSqlite(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
{
    if (libraryName == "e_sqlite3" || libraryName.Contains("sqlite3"))
    {
        // 1. Tenta carregar do diretório da aplicação (bin/Debug/net10.0/e_sqlite3.dll)
        var localPath = Path.Combine(AppContext.BaseDirectory, "e_sqlite3.dll");
        if (File.Exists(localPath))
        {
            return NativeLibrary.Load(localPath);
        }

        // 2. Tenta carregar da raiz do projeto (Atividade/e_sqlite3.dll)
        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "e_sqlite3.dll");
        if (File.Exists(rootPath))
        {
            return NativeLibrary.Load(rootPath);
        }

        // 3. Tenta carregar da pasta nativa win-x64
        var winx64Path = Path.Combine(AppContext.BaseDirectory, "runtimes", "win-x64", "native", "e_sqlite3.dll");
        if (File.Exists(winx64Path))
        {
            return NativeLibrary.Load(winx64Path);
        }

        // 4. Fallback para a DLL SQLite nativa do próprio Windows (System32)
        var sysPath = Path.Combine(Environment.SystemDirectory, "winsqlite3.dll");
        if (File.Exists(sysPath))
        {
            return NativeLibrary.Load(sysPath);
        }
    }
    return IntPtr.Zero;
}

// Configura o resolver em todos os assemblies SQLite
try
{
    // Força o carregamento do provedor e vincula o resolvedor
    var providerAsm = typeof(SQLitePCL.SQLite3Provider_e_sqlite3).Assembly;
    NativeLibrary.SetDllImportResolver(providerAsm, ResolveSqlite);
    SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
}
catch (Exception ex)
{
    Console.WriteLine($"[AVISO] Configuração de DLL SQLite: {ex.Message}");
}

var builder = WebApplication.CreateBuilder(args);

// Configuração de Data Protection local no diretório do projeto
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, ".keys");
Directory.CreateDirectory(keysFolder);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));

// Configuração de Serviços MVC e Injeção de Dependências
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".LojaCupcakes.Session";
});

builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddScoped<CarrinhoService>();

var app = builder.Build();

// Pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Validação imediata de persistência na inicialização
try
{
    var db = app.Services.GetRequiredService<DatabaseHelper>();
    var produtosIniciais = db.ObterProdutos();
    Console.WriteLine($"[INICIALIZACAO] Banco SQLite conectado com sucesso! {produtosIniciais.Count} cupcakes carregados.");
}
catch (Exception ex)
{
    Console.WriteLine($"[ERRO INICIALIZACAO SQLITE] {ex.GetType().Name}: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"  InnerException: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
    }
}

// Execução de testes automatizados via linha de comando (ex: dotnet run -- --test)
if (args.Contains("--test") || args.Contains("test"))
{
    var dbHelper = app.Services.GetRequiredService<DatabaseHelper>();
    int falhas = SuiteTestesUnitarios.ExecutarTodosOsTestes(dbHelper);
    Environment.Exit(falhas == 0 ? 0 : 1);
    return;
}

app.Run();
