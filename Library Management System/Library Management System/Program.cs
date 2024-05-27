using Library_Management_System;
using Library_Management_System.Repositories.Adaptors;
using Library_Management_System.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DBConfig>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();

builder.Services.AddControllers();
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
