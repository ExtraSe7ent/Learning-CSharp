using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Student
{
    public string Name { get; set; }
    public List<CourseRegistration> RegisteredCourses { get; set; }

    public Student(string name)
    {
        Name = name;
        RegisteredCourses = new List<CourseRegistration>();
    }

    public bool AddCourse(string courseName, int semester)
    {
        foreach (var registration in RegisteredCourses)
        {
            if (registration.CourseName.Equals(courseName, StringComparison.OrdinalIgnoreCase) && registration.Semester == semester)
            {
                return false;
            }
        }
        RegisteredCourses.Add(new CourseRegistration(courseName, semester));
        return true;
    }
}

public class CourseRegistration
{
    public string CourseName { get; set; }
    public int Semester { get; set; }

    public CourseRegistration(string courseName, int semester)
    {
        CourseName = courseName;
        Semester = semester;
    }

    public override string ToString()
    {
        return $"Mon: {CourseName,-7} | Hoc ky: {Semester}";
    }
}

public class StudentManager
{
    private List<Student> studentList;
    private string filePath;

    public StudentManager(string path)
    {
        filePath = path;
        studentList = new List<Student>();
        LoadStudentsFromFile();
    }

    public void LoadStudentsFromFile()
    {
        if (!File.Exists(filePath)) return;
        studentList.Clear();
        var lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length == 3 && int.TryParse(parts[1].Trim(), out int semester))
            {
                string name = parts[0].Trim();
                string course = parts[2].Trim();
                Student student = FindStudentByName(name);
                if (student == null)
                {
                    student = new Student(name);
                    studentList.Add(student);
                }
                student.AddCourse(course, semester);
            }
        }
        Console.WriteLine($"Da tai thong tin cua {studentList.Count} sinh vien tu file.");
    }

    private void SaveChangesToFile()
    {
        var lines = new List<string>();
        foreach (var student in studentList.OrderBy(s => s.Name))
        {
            foreach (var course in student.RegisteredCourses)
            {
                lines.Add($"{student.Name},{course.Semester},{course.CourseName}");
            }
        }
        File.WriteAllLines(filePath, lines);
    }

    public Student FindStudentByName(string name)
    {
        foreach (Student student in studentList)
        {
            if (student.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return student;
            }
        }
        return null;
    }

    public bool AddRegistration(string name, int semester, string courseName)
    {
        Student student = FindStudentByName(name);
        if (student == null)
        {
            student = new Student(name);
            studentList.Add(student);
        }
        bool result = student.AddCourse(courseName, semester);
        if (result) SaveChangesToFile();
        return result;
    }

    public bool DeleteStudent(string name)
    {
        var student = FindStudentByName(name);
        if (student != null)
        {
            studentList.Remove(student);
            SaveChangesToFile();
            return true;
        }
        return false;
    }

    public bool UpdateRegistration(string studentName, int courseIndex, string newCourseName, int newSemester)
    {
        Student student = FindStudentByName(studentName);
        if (student == null || courseIndex < 0 || courseIndex >= student.RegisteredCourses.Count)
        {
            return false;
        }

        for (int i = 0; i < student.RegisteredCourses.Count; i++)
        {
            if (i == courseIndex) continue;
            var reg = student.RegisteredCourses[i];
            if (reg.CourseName.Equals(newCourseName, StringComparison.OrdinalIgnoreCase) && reg.Semester == newSemester)
            {
                Console.WriteLine("Loi: Thong tin moi bi trung voi mot dang ky da co.");
                return false;
            }
        }

        student.RegisteredCourses[courseIndex].CourseName = newCourseName;
        student.RegisteredCourses[courseIndex].Semester = newSemester;
        SaveChangesToFile();
        return true;
    }

    public void PrintRegistrationReport()
    {
        if (studentList.Count == 0)
        {
            Console.WriteLine("Khong co du lieu de thong ke.");
            return;
        }

        var reportData = new Dictionary<string, Dictionary<string, int>>();

        foreach (var student in studentList)
        {
            foreach (var registration in student.RegisteredCourses)
            {
                string courseName = registration.CourseName;
                string studentName = student.Name;

                if (!reportData.ContainsKey(courseName))
                {
                    reportData[courseName] = new Dictionary<string, int>();
                }

                if (!reportData[courseName].ContainsKey(studentName))
                {
                    reportData[courseName][studentName] = 1;
                }
                else
                {
                    reportData[courseName][studentName]++;
                }
            }
        }

        Console.WriteLine("\nStudent Name      | Course     | Total of Course");
        Console.WriteLine("  ------------------|------------|-----------------  ");

        var sortedCourses = reportData.Keys.OrderBy(c => c);

        foreach (var courseName in sortedCourses)
        {
            var studentCounts = reportData[courseName];
            var sortedStudents = studentCounts.Keys.OrderBy(s => s);
            
            foreach(var studentName in sortedStudents)
            {
                int count = studentCounts[studentName];
                Console.WriteLine($"{studentName,-17} | {courseName,-10} | {count}");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        StudentManager manager = new StudentManager("Students.txt");

        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\n1. Them sinh vien");
            Console.WriteLine("2. Sua thong tin sinh vien");
            Console.WriteLine("3. Xoa sinh vien");
            Console.WriteLine("4. Tim kiem sinh vien");
            Console.WriteLine("5. Xem thong ke");
            Console.WriteLine("6. Thoat");
            Console.Write("Vui long chon chuc nang: ");

            switch (Console.ReadLine())
            {
                case "1": AddStudents(manager); break;
                case "2": UpdateRegistration(manager); break;
                case "3": RemoveStudents(manager); break;
                case "4": SearchStudent(manager); break;
                case "5": manager.PrintRegistrationReport(); break;
                case "6": isRunning = false; break;
                default: Console.WriteLine("Lua chon khong hop le."); break;
            }
        }
    }

    static void AddStudents(StudentManager manager)
    {
        int count;
        while (true)
        {
            Console.Write("\n - Nhap so luong luot dang ky ban muon them: ");
            if (int.TryParse(Console.ReadLine(), out count) && count > 0)
            {
                break;
            }
            Console.WriteLine("So luong khong hop le. Vui long nhap mot so lon hon 0.");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n - Them luot dang ky thu {i + 1}/{count}");

            string name;
            while (true)
            {
                Console.Write("Nhap ten sinh vien: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.WriteLine("Ten sinh vien khong duoc de trong. Vui long nhap lai.");
            }

            int semester;
            while (true)
            {
                Console.Write("Nhap hoc ky: ");
                if (int.TryParse(Console.ReadLine(), out semester) && semester > 0)
                {
                    break;
                }
                Console.WriteLine("Hoc ky khong hop le. Vui long nhap so nguyen duong.");
            }

            string courseName;
            var allowedCourses = new List<string> { "Java", ".Net", "C/C++" };
            while (true)
            {
                Console.Write("Nhap ten mon hoc (Java, .Net, C/C++): ");
                courseName = Console.ReadLine();

                bool isValidCourse = false;
                foreach (string allowed in allowedCourses)
                {
                    if (allowed.Equals(courseName, StringComparison.OrdinalIgnoreCase))
                    {
                        courseName = allowed;
                        isValidCourse = true;
                        break;
                    }
                }

                if (isValidCourse)
                {
                    break;
                }

                Console.WriteLine("Ten mon hoc khong hop le. Vui long nhap lai.");
            }

            if (manager.AddRegistration(name, semester, courseName))
            {
                Console.WriteLine("\nThem thanh cong!");
            }
            else
            {
                Console.WriteLine("\nLuot dang ky nay da ton tai, khong them.");
            }
        }
    }

    static void RemoveStudents(StudentManager manager)
    {
        int count;
        while (true)
        {
            Console.Write("\n - Nhap so luong sinh vien ban muon xoa: ");
            if (int.TryParse(Console.ReadLine(), out count) && count > 0)
            {
                break;
            }
            Console.WriteLine("So luong khong hop le. Vui long nhap mot so lon hon 0.");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n - Xoa sinh vien thu {i + 1}/{count}");

            string name;
            while (true)
            {
                Console.Write("Nhap ten sinh vien can xoa: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.WriteLine("Ten sinh vien khong duoc de trong. Vui long nhap lai.");
            }

            if (manager.DeleteStudent(name))
            {
                Console.WriteLine($"Da xoa thanh cong sinh vien '{name}'.");
            }
            else
            {
                Console.WriteLine($"Khong tim thay sinh vien '{name}'.");
            }
        }
    }

    static void SearchStudent(StudentManager manager)
    {
        string name;
        while (true)
        {
            Console.Write("\nNhap ten sinh vien can tim: ");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                break;
            }
            Console.WriteLine("Ten sinh vien khong duoc de trong. Vui long nhap lai.");
        }

        var student = manager.FindStudentByName(name);
        if (student != null)
        {
            Console.WriteLine($"\nThong tin sinh vien: {student.Name}");
            Console.WriteLine("Cac mon da dang ky (sap xep theo hoc ky):");
            if (student.RegisteredCourses.Any())
            {
                foreach (var course in student.RegisteredCourses.OrderBy(c => c.Semester))
                {
                    Console.WriteLine($"- {course}");
                }
            }
            else
            {
                Console.WriteLine("Chua dang ky mon nao.");
            }
        }
        else
        {
            Console.WriteLine($"Khong tim thay sinh vien nao co ten '{name}'.");
        }
    }

    static void UpdateRegistration(StudentManager manager)
    {
        Console.WriteLine("\n - Sua thong tin sinh vien");

        string name;
        while (true)
        {
            Console.Write("Nhap ten sinh vien can sua: ");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) break;
            Console.WriteLine("Ten khong duoc de trong.");
        }

        Student student = manager.FindStudentByName(name);
        if (student == null)
        {
            Console.WriteLine($"Khong tim thay sinh vien '{name}'.");
            return;
        }

        if (student.RegisteredCourses.Count == 0)
        {
            Console.WriteLine($"Sinh vien '{name}' chua dang ky mon nao.");
            return;
        }

        Console.WriteLine("\nDanh sach cac luot dang ky cua sinh vien:");
        for (int i = 0; i < student.RegisteredCourses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {student.RegisteredCourses[i]}");
        }

        int choice;
        while (true)
        {
            Console.Write("Chon phan dang ky ban muon sua (nhap so): ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= student.RegisteredCourses.Count)
            {
                break;
            }
            Console.WriteLine("Lua chon khong hop le.");
        }
        int courseIndex = choice - 1;

        Console.WriteLine("\nNhap thong tin moi cho luot dang ky nay:");
        int newSemester;
        while (true)
        {
            Console.Write("Hoc ky moi: ");
            if (int.TryParse(Console.ReadLine(), out newSemester) && newSemester > 0) break;
            Console.WriteLine("Hoc ky khong hop le.");
        }

        string newCourseName;
        var allowedCourses = new List<string> { "Java", ".Net", "C/C++" };
        while (true)
        {
            Console.Write("Ten mon hoc moi (Java, .Net, C/C++): ");
            newCourseName = Console.ReadLine();

            bool isValidCourse = false;
            foreach (string allowed in allowedCourses)
            {
                if (allowed.Equals(newCourseName, StringComparison.OrdinalIgnoreCase))
                {
                    newCourseName = allowed;
                    isValidCourse = true;
                    break;
                }
            }

            if (isValidCourse)
            {
                break;
            }
            Console.WriteLine("Ten mon hoc khong hop le.");
        }

        if (manager.UpdateRegistration(name, courseIndex, newCourseName, newSemester))
        {
            Console.WriteLine("\nCap nhat thanh cong!");
        }
        else
        {
            Console.WriteLine("\nCap nhat that bai. Vui long thu lai.");
        }
    }
}
