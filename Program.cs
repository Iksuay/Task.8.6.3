string folderPath = @"C:\Users\tamer\Рабочий стол\TrashFolderSize";

if (Directory.Exists(folderPath))
{
    DirectoryInfo dir = new DirectoryInfo(folderPath);
    DateTime currentTime = DateTime.Now;
    long folderSize = FolderSize(folderPath);
    FolderSize(folderPath);
    Console.WriteLine($"Исходный размер папки: {folderPath} {folderSize} байт");
    DeleteFilesAndFolders(dir, currentTime);
    long freedSize = folderSize - FolderSize(folderPath);
    Console.WriteLine($"Освобождено {freedSize} байт");
    folderSize = FolderSize(folderPath);
    Console.WriteLine($"После очистки размер папки: {folderPath} {folderSize} байт");
}
else
{
    Console.WriteLine("Папка не существует: " + folderPath);
}

static long FolderSize(string folderPath)
{
    long folderSize = 0;
    DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
    if (!directoryInfo.Exists)
    {
        throw new DirectoryNotFoundException($"Папка {folderPath} не найдена");
    }
    FileInfo[] files = directoryInfo.GetFiles();
    foreach (FileInfo file in files)
    {
        folderSize += file.Length;
    }

    DirectoryInfo[] subDirs = directoryInfo.GetDirectories();
    foreach (DirectoryInfo subDir in subDirs)
    {
        folderSize += FolderSize(subDir.FullName);
    }
    return folderSize;
}

static void DeleteFilesAndFolders(DirectoryInfo dir, DateTime currentTime)
{
    foreach (FileInfo file in dir.GetFiles())
    {
        TimeSpan timeLastAccess = currentTime - file.LastAccessTime;
        if (timeLastAccess.TotalMinutes > 0)
        {
            Console.WriteLine("Удаляем файл: " + file.Name);
            try
            {
                file.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении файла: " + ex.Message);
            }
        }
    }

    foreach (DirectoryInfo subDir in dir.GetDirectories())
    {
        DeleteFilesAndFolders(subDir, currentTime);
        if (subDir.GetFiles().Length == 0 && subDir.GetDirectories().Length == 0)
        {
            Console.WriteLine("Удаляем пустую папку: " + subDir.Name);
            try
            {
                subDir.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении папки: " + ex.Message);
            }
        }
    }
}