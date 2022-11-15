using DOTNETCOREEXAMPLE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNETCOREEXAMPLE.DataContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
        }
        public virtual DbSet<Class_PROJECT_USER_MASTER> obj_PROJECT_USER_MASTER { get; set; }
        public virtual DbSet<Class_PATIENT_MASTER> obj_PATIENT_MASTER { get; set; }
        public virtual DbSet<Class_REGISTRATION_TYPE> obj_REGISTRATION_TYPE { get; set; }
        public virtual DbSet<Class_SLOT_MASTER> obj_SLOT_MASTER { get; set; }
        public virtual DbSet<Class_SERVICE_GROUP_MASTER> obj_SERVICE_GROUP_MASTER { get; set; }
        public virtual DbSet<Class_SERVICE_MASTER> obj_SERVICE_MASTER { get; set; }
        public virtual DbSet<Class_VISIT_TYPE_MASTER> obj_VISIT_TYPE_MASTER { get; set; }
        public virtual DbSet<Class_SLOT_BOOKING_MASTER> obj_SLOT_BOOKING_MASTER { get; set; }
        public virtual DbSet<Class_OTP_MASTER> obj_OTP_MASTER { get; set; }
       
    }
}
