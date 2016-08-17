namespace NzzApp.Model.Implementation.Images
{
    internal class DensityAwareSize
    {
        protected readonly int Density;

        internal DensityAwareSize(int density)
        {
            Density = density;
        }

        internal string GetGalleryHeight()
        {
            //if (Density < 160)
            //{
            //    return "203";
            //}
            if (Density <= 160)
            {
                return "270";
            }
            if (Density <= 240)
            {
                return "270";
            }
            
            // _density > 240
            return "405";
        }

        internal string GetTopHeight()
        {
            //if (Density < 160)
            //{
            //    return "135";
            //}
            if (Density <= 160)
            {
                return "180";
            }
            if (Density <= 240)
            {
                return "270";
            }
            
            // _density > 240
            return "360";
        }

        internal string GetSquareHeight()
        {
            //if (Density < 160)
            //{
            //    return "73";
            //}
            if (Density <= 160)
            {
                return "97";
            }
            if (Density <= 240)
            {
                return "145";
            }
            
            // _density > 240
            return "193";
        }

        internal virtual string GetGalleryWidth()
        {
            //if (Density < 160)
            //{
            //    return "-";
            //}
            if (Density <= 160)
            {
                return "-";
            }
            if (Density <= 240)
            {
                return "-";
            }
            
            // _density > 240
            return "-";
        }

        internal virtual string GetTopWidth()
        {
            //if (Density < 160)
            //{
            //    return "240";
            //}
            if (Density <= 160)
            {
                return "320";
            }
            if (Density <= 240)
            {
                return "480";
            }
            
            // _density > 240
            return "640";
        }

        internal virtual string GetSquareWidth()
        {
            //if (Density < 160)
            //{
            //    return "73";
            //}
            if (Density <= 160)
            {
                return "97";
            }
            if (Density <= 240)
            {
                return "145";
            }
            
            // _density > 240
            return "193";
        }

        internal string GetGalleryFormat()
        {
            return "text";
        }

        internal string GetTopFormat()
        {
            return "text";
        }

        internal string GetSquareFormat()
        {
            return "lead";
        }
    }
}