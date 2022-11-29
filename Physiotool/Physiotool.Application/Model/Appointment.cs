using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Physiotool.Application.Model
{
    /// <summary>
    /// Der Termin des Patienten.
    /// </summary>
    [Table("Appointment")]
    public class Appointment
    {
        public Appointment(DateTime date, TimeSpan time, Patient patient, DateTime created)
        {
            Date = date.Date;
            Time = time;
            PatientId = patient.Id;
            Patient = patient;
            Created = created;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Appointment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime Created { get; set; }
    }

    public class ConfirmedAppointment : Appointment
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected ConfirmedAppointment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ConfirmedAppointment(DateTime date, TimeSpan time, Patient patient, DateTime created, TimeSpan duration)
            : base(date, time, patient, created)
        {
            Duration = duration;
        }
        public TimeSpan Duration { get; set; }
    }
    public class DeletedAppointment : Appointment
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected DeletedAppointment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DeletedAppointment(DateTime date, TimeSpan time, Patient patient, DateTime created)
            : base(date, time, patient, created) { }
    }
}
