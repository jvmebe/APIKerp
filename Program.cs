using APIKerp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddScoped<ClienteRepository>(provider =>
    new ClienteRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PaisRepository>(provider =>
    new PaisRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EstadoRepository>(provider =>
    new EstadoRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FornecedorRepository>(provider =>
    new FornecedorRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CidadeRepository>(provider =>
    new CidadeRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ListaRepository>(provider =>
    new ListaRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<RegiaoRepository>(provider =>
    new RegiaoRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<RamoAtividadesRepository>(provider =>
    new RamoAtividadesRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CondPagRepository>(provider =>
    new CondPagRepository(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
