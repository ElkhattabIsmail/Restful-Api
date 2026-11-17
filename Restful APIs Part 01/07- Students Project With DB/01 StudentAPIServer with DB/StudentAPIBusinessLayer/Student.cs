using System.Data;
using StudentDataAccessLayer;

namespace StudentAPIBusinessLayer
{

    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public UpdatedStudentDTO uSDTO
        {
            get { return (new UpdatedStudentDTO(this.Name, this.Age, this.Grade)); }
        }
        public StudentDTO SDTO {
            get { return (new StudentDTO(this.ID, this.Name, this.Age, this.Grade)); }
        }

            public int ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Grade { get; set; }

            public Student(StudentDTO SDTO, enMode cMode =enMode.AddNew )

            {
                this.ID = SDTO.Id ;
                this.Name =SDTO.Name; 
                this.Age =SDTO.Age;
                this.Grade = SDTO.Grade;
             
                Mode = cMode;
            }
        public Student(UpdatedStudentDTO uSDTO, enMode cMode = enMode.AddNew)

        {
            this.Name = uSDTO.Name;
            this.Age = uSDTO.Age;
            this.Grade = uSDTO.Grade;

            Mode = cMode;
        }
        // Simple Test.
        public StudentNameDTO S_Name
        {
            get { return (new StudentNameDTO(this.Name)); }
        }
        public Student(StudentNameDTO SName)
        {
            this.Name = SName.Name;
        }

        public static StudentNameDTO GetStudentNameById(int id)
        {
            //call DataAccess Layer 

            var Sname = StudentData.GetStudentName(id);

            return Sname;
        }
        
        private bool _AddNewStudent()
            {
                //call DataAccess Layer 

                this.ID = StudentData.AddStudent(SDTO);

                return (this.ID != -1);
            }

            private bool _UpdateStudent()
            {
                return StudentData.UpdateStudent(this.ID ,uSDTO);
            }


            public static List<StudentDTO> GetAllStudents()
            {
                return StudentData.GetAllStudents();
            }


        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public static Student Find(int ID)
            {

               StudentDTO SDTO = StudentData.GetStudentById(ID);

            if (SDTO != null)
            //we return new object of that student with the right data
            {
                
                return new Student(SDTO, enMode.Update);
            }
            else
                return null;
            }

            public bool Save()
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewStudent())
                        {

                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case enMode.Update:

                        return _UpdateStudent();

                }

                return false;
            }

            public static bool DeleteStudent(int ID)
            {
                return StudentData.DeleteStudent(ID);
            }

        }
    }

