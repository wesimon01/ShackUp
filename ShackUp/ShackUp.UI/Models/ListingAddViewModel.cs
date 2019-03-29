using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ShackUp.UI.Models
{
    public class ListingAddViewModel : IValidatableObject
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> BathroomTypes { get; set; }
        public Listing Listing { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Listing.Nickname))            
                errors.Add(new ValidationResult("Nickname is required"));
            
            if (string.IsNullOrEmpty(Listing.City))           
                errors.Add(new ValidationResult("City is required"));
            
            if (string.IsNullOrEmpty(Listing.ListingDescription))            
                errors.Add(new ValidationResult("Description is required"));
            
            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                var extensions = new string[] { ".jpg", ".png", ".jpeg", ".gif" };
                var extension = Path.GetExtension(ImageUpload.FileName);

                if (!extensions.Contains(extension))
                    errors.Add(new ValidationResult($"Image file must be a jpg, png, jpeg, or gif"));               
            }
            else {
                errors.Add(new ValidationResult("Image file is required"));
            }

            if (Listing.Rate <= 0)
                errors.Add(new ValidationResult("Rate must be greater than zero"));

            if (Listing.SquareFootage <= 0)
                errors.Add(new ValidationResult("Square footage must be greater than zero"));


            return errors;
        }
    }
}