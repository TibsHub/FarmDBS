using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Crop
    {
        // RK atribiutes

        private string cropType;
        private string cropName;
        private float cropAmount;
        private string cropInCultivation;
        private float cropStorageTempMin;
        private float cropStorageTempMax;
        private string cropStorageType;

        // RK constructor

        public Crop(string cropName, string cropType, float cropAmount, string cropInCultivation, float cropStorageTempMin, float cropStorageMax, string cropStorageType)
        {
            this.CropType = cropType;
            this.CropName = cropName;
            this.CropAmount = cropAmount;
            this.CropInCultivation = cropInCultivation;
            this.CropStorageTempMin = cropStorageTempMin;
            this.CropStorageTempMax = cropStorageMax;
            this.CropStorageType = cropStorageType;
        }

        // RK properties

        public string CropType
        {
            get { return cropType; }
            set { cropType = value; }
        }

        public string CropName
        {
            get { return cropName; }
            set { cropName = value; }
        }

        public float CropAmount
        {
            get { return cropAmount; }
            set { cropAmount = value; }
        }

        public string CropInCultivation
        {
            get { return cropInCultivation; }
            set { cropInCultivation = value; }
        }

        public float CropStorageTempMin
        {
            get { return cropStorageTempMin; }
            set { cropStorageTempMin = value; }
        }

        public float CropStorageTempMax
        {
            get { return cropStorageTempMax; }
            set { cropStorageTempMax = value; }
        }


        public string CropStorageType
        {
            get { return cropStorageType; }
            set { cropStorageType = value; }
        }  
    }
}
