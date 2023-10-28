using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;
using Microsoft.EntityFrameworkCore;

//const string connectionString = "Data Source=C:/Users/droll/Desktop/DiplomFiles/SSU.DepManagement/SSU.DepManagement.WebAssembly/Server/Storage.db";
const string connectionString = "Host=localhost;Port=5432;Database=storage;Username=user;Password=password";
Console.WriteLine($"This app inserts file into files storage.{Environment.NewLine}" +
                  $"Connection string: {connectionString}");
var path = ReadValue("File path: ", "File not exists!", File.Exists, s => s);
var key = ReadValue("File key: ", "Key must be not empty!", key => !string.IsNullOrEmpty(key), s => s);

var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
optionsBuilder.UseNpgsql(connectionString);
var ctx = new ApplicationContext(optionsBuilder.Options);
var filesStorage = new FilesStorageDao(ctx);
filesStorage.Save(key, Path.GetFileName(path), await File.ReadAllBytesAsync(path));
Console.WriteLine("Database successfully updated!");
return;


T ReadValue<T>(string message, string errorMessage, Predicate<string> valid, Func<string, T> convert)
{
    string value;
    bool retry;
    do
    {
        retry = false;
        Console.Write(message);
        value = Console.ReadLine();
        if (!valid(value))
        {
            Console.WriteLine(errorMessage);
            retry = true;
        }
    } while (retry);

    return convert(value);
}


