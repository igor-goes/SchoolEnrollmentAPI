using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolEnrollmentAPI.Models;
using SchoolEnrollmentAPI.Repositories;
using SchoolEnrollmentAPI.Services;

namespace SchoolEnrollmentAPI.Tests
{
    /// <summary>
    /// Valida as regras de negócio da matrícula sem depender de SQL Server.
    /// </summary>
    [TestClass]
    public class EnrollmentServiceTests
    {
        [TestMethod]
        public void Create_ShouldReturnEnrollment_WhenRepositoryCreatesEnrollment()
        {
            var service = CreateService(EnrollmentResult.Created);

            var enrollment = service.Create(new EnrollmentRequest { StudentId = 1, ClassroomId = 2 });

            Assert.AreEqual(1, enrollment.StudentId);
            Assert.AreEqual(2, enrollment.ClassroomId);
        }

        [TestMethod]
        public void Create_ShouldThrowConflict_WhenStudentIsInactive()
        {
            var service = CreateService(EnrollmentResult.StudentInactive);

            var exception = Assert.ThrowsException<EnrollmentException>(
                () => service.Create(new EnrollmentRequest { StudentId = 1, ClassroomId = 2 }));

            Assert.AreEqual(HttpStatusCode.Conflict, exception.StatusCode);
        }

        [TestMethod]
        public void Create_ShouldThrowConflict_WhenClassroomHasNoRemainingSeats()
        {
            var service = CreateService(EnrollmentResult.NoRemainingSeats);

            var exception = Assert.ThrowsException<EnrollmentException>(
                () => service.Create(new EnrollmentRequest { StudentId = 1, ClassroomId = 2 }));

            Assert.AreEqual(HttpStatusCode.Conflict, exception.StatusCode);
        }

        [TestMethod]
        public void Create_ShouldThrowConflict_WhenEnrollmentAlreadyExists()
        {
            var service = CreateService(EnrollmentResult.DuplicateEnrollment);

            var exception = Assert.ThrowsException<EnrollmentException>(
                () => service.Create(new EnrollmentRequest { StudentId = 1, ClassroomId = 2 }));

            Assert.AreEqual(HttpStatusCode.Conflict, exception.StatusCode);
        }

        [TestMethod]
        public void Create_ShouldThrowNotFound_WhenStudentDoesNotExist()
        {
            var service = CreateService(EnrollmentResult.StudentNotFound);

            var exception = Assert.ThrowsException<EnrollmentException>(
                () => service.Create(new EnrollmentRequest { StudentId = 1, ClassroomId = 2 }));

            Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);
        }

        private static EnrollmentService CreateService(EnrollmentResult enrollmentResult)
        {
            // O repositório falso controla o cenário sem tocar no banco de dados.
            return new EnrollmentService(new FakeEnrollmentRepository(enrollmentResult));
        }
    }

    /// <summary>
    /// Simula o resultado do repositório para isolar os testes das regras do serviço.
    /// </summary>
    internal class FakeEnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnrollmentResult _enrollmentResult;

        public FakeEnrollmentRepository(EnrollmentResult enrollmentResult)
        {
            _enrollmentResult = enrollmentResult;
        }

        public EnrollmentResult Create(int studentId, int classroomId)
        {
            return _enrollmentResult;
        }
    }
}
