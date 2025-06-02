
using AssignmentManagement.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
builder.Services.AddSingleton<IAppLogger, ConsoleAppLogger>();
builder.Services.AddSingleton<IAssignmentService, AssignmentService>();

var app = builder.Build();

app.MapControllers();
app.Run();

public partial class Program { }
