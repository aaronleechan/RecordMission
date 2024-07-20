namespace RecordMission;

public class Constants
{
    private const string DBFileName = "RecordMission.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath
    {
        get
        {
            var basePath = FileSystem.AppDataDirectory;
            return Path.Combine(basePath, DBFileName);
        }
    }
}
