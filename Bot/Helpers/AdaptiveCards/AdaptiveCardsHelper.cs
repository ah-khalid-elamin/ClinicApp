using AdaptiveCards;
using Common.Models;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace Bot.Helpers.AdaptiveCards
{
    public class AdaptiveCardsHelper
    {
        public static Attachment GetDoctorCard(Doctor doctor)
        {
            var card = new AdaptiveCard();

            AdaptiveColumnSet columnSet = new AdaptiveColumnSet();
            columnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = "0.5",
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveImage()
                    {
                        PixelWidth = 100,
                        PixelHeight = 100,
                        AltText = "Doctor Image",
                        Url = new System.Uri("https://img.icons8.com/external-justicon-flat-justicon/344/external-doctor-hospital-and-medical-justicon-flat-justicon.png")
                    }
                }
            });
            columnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = AdaptiveColumnWidth.Stretch,
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = doctor.Name.ToUpper(),
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Medium,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"{doctor.Speciality}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible= true,
                        IsSubtle = true,
                    }                }
            });


            card.Body.Add(
                 columnSet
                );

            return new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCard.ContentType
            };

        }
        public static Attachment GetAppointmentCard(Appointment appointment)
        {
            var card = new AdaptiveCard();


            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"Id: {appointment.Id}",
                Wrap = false,
                Spacing = AdaptiveSpacing.Small,
                Size = AdaptiveTextSize.Default,
                Height = AdaptiveHeight.Stretch,
                FontType = AdaptiveFontType.Default,
                Weight = AdaptiveTextWeight.Default,
                Color = AdaptiveTextColor.Default,
                IsVisible = true,
                IsSubtle = true,
            });
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"Date: {appointment.StartDate.ToShortDateString()}",
                Wrap = false,
                Spacing = AdaptiveSpacing.Small,
                Size = AdaptiveTextSize.Default,
                Height = AdaptiveHeight.Stretch,
                FontType = AdaptiveFontType.Default,
                Weight = AdaptiveTextWeight.Default,
                Color = AdaptiveTextColor.Default,
                IsVisible = true,
                IsSubtle = true,

            });

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"From: {appointment.StartDate.ToShortTimeString()} - {appointment.EndDate.ToShortTimeString()}",
                Wrap = false,
                Spacing = AdaptiveSpacing.Small,
                Size = AdaptiveTextSize.Default,
                Height = AdaptiveHeight.Stretch,
                FontType = AdaptiveFontType.Default,
                Weight = AdaptiveTextWeight.Default,
                Color = AdaptiveTextColor.Default,
                IsVisible = true,
                IsSubtle = true,

            });

            //paptientColumnSet
            AdaptiveColumnSet patientcolumnSet = new AdaptiveColumnSet();
            patientcolumnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Spacing = AdaptiveSpacing.Large,
                Width = "0.4",
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveImage()
                    {
                        PixelWidth = 100,
                        PixelHeight = 100,
                        AltText = "Patient Image",
                        Url = new System.Uri("https://img.icons8.com/external-smashingstocks-circular-smashing-stocks/344/external-patient-cancer-survivors-day-smashingstocks-circular-smashing-stocks.png")
                    }
                }
            });
            patientcolumnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = AdaptiveColumnWidth.Auto,
                Spacing = AdaptiveSpacing.Large,
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = appointment.Patient.Name.ToUpper(),
                        Wrap = false,
                        Spacing = AdaptiveSpacing.Small,
                        Size = AdaptiveTextSize.Medium,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"Birth: {appointment.Patient.BirthDate.ToShortDateString()}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible= true,
                        IsSubtle = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"{appointment.Patient.Gender}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsSubtle= true,
                    }
                }
            });

            AdaptiveColumnSet doctorcolumnSet = new AdaptiveColumnSet();
            doctorcolumnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = "0.5",
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveImage()
                    {
                        PixelWidth = 100,
                        PixelHeight = 100,
                        AltText = "Doctor Image",
                        Url = new System.Uri("https://img.icons8.com/external-justicon-flat-justicon/344/external-doctor-hospital-and-medical-justicon-flat-justicon.png")
                    }
                }
            });
            doctorcolumnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = AdaptiveColumnWidth.Stretch,
                Spacing = AdaptiveSpacing.Small,
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = appointment.Doctor.Name.ToUpper(),
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Medium,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"{appointment.Doctor.Speciality}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible= true,
                        IsSubtle = true,
                    }                }
            });


            card.Body.Add(
                 patientcolumnSet
                );
            card.Body.Add(
                 doctorcolumnSet
                );



            return new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCard.ContentType
            };

        }
        public static Attachment GetPatientCard(Patient patient)
        {
            var card = new AdaptiveCard();

            AdaptiveColumnSet columnSet = new AdaptiveColumnSet();
            columnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = "0.4",
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveImage()
                    {
                        PixelWidth = 100,
                        PixelHeight = 100,
                        AltText = "Patient Image",
                        Url = new System.Uri("https://img.icons8.com/external-smashingstocks-circular-smashing-stocks/344/external-patient-cancer-survivors-day-smashingstocks-circular-smashing-stocks.png")
                    }
                }
            });
            columnSet.Columns.Add(new AdaptiveColumn()
            {
                Type = AdaptiveColumn.TypeName,
                Width = AdaptiveColumnWidth.Stretch,
                Items = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = patient.Name.ToUpper(),
                        Wrap = false,
                        Spacing = AdaptiveSpacing.Small,
                        Size = AdaptiveTextSize.Medium,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"Birth: {patient.BirthDate.ToShortDateString()}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsVisible= true,
                        IsSubtle = true,
                    },
                    new AdaptiveTextBlock()
                    {
                        Text = $"{patient.Gender}",
                        Wrap = false,
                        Spacing = AdaptiveSpacing.None,
                        Size = AdaptiveTextSize.Default,
                        Height = AdaptiveHeight.Auto,
                        FontType = AdaptiveFontType.Default,
                        Weight = AdaptiveTextWeight.Default,
                        Color = AdaptiveTextColor.Default,
                        IsSubtle= true,
                    }
                }
            }) ;


            card.Body.Add(
                 columnSet
                );

            return new Attachment()
            {
                Content = card,
                ContentType = AdaptiveCard.ContentType
            };
        }
        public static List<Attachment> GetDoctorsCards(List<Doctor> doctors)
        {
            var cards = new List<Attachment>();
            foreach (var doctor in doctors)
            {
                cards.Add(GetDoctorCard(doctor));
            }
            return cards;
        }
        public static List<Attachment> GetPatientsCards(List<Patient> patients)
        {
            var cards = new List<Attachment>();
            foreach (var patient in patients)
            {
                cards.Add(GetPatientCard(patient));
            }
            return cards;
        }
        public static List<Attachment> GetAppointmentsCards(List<Appointment> appointments)
        {
            var cards = new List<Attachment>();
            foreach (var appointment in appointments)
            {
                cards.Add(GetAppointmentCard(appointment));
            }
            return cards;
        }
        public static Attachment GetUnsupportedOperationCard()
        {
            AdaptiveCard card = new("1.2");
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = $"Unsupported Operation",
                Wrap = false,
                Spacing = AdaptiveSpacing.None,
                Size = AdaptiveTextSize.Default,
                Height = AdaptiveHeight.Stretch,
                FontType = AdaptiveFontType.Default,
                Weight = AdaptiveTextWeight.Default,
                Color = AdaptiveTextColor.Default
            });

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };

        }



    }
}
