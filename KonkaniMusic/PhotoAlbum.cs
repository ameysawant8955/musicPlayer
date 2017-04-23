using System;

namespace KonkaniMusic
{
    public class Photo
    {
        // Photo ID for this photo:
        public int mPhotoID;

        // Caption text for this photo:
        public string mCaption;

        public string mArtist;

        public string songUrl;

        public string imageUrl;

        // Return the ID of the photo:
        public int PhotoID
        {
            get { return mPhotoID; }
        }

        // Return the Caption of the photo:
        public string Caption
        {
            get { return mCaption; }
        }
    }
    public class PhotoAlbum
    {
        // Built-in photo collection - this could be replaced with
        // a photo database:

        static Photo[] mBuiltInPhotos = {
            new Photo { mPhotoID = Resource.Drawable.m1,imageUrl="https://static.blugaa.com/thumbs/100_100/jbvai.jpg",
                        mCaption = "Tu Hai Unplugged ",mArtist="AR Rahman",songUrl="http://dl.jatt.link/lq.jatt.link/cdn8/ddfdd14f0dbcc3a2d2b2d76af63334c2/ietzv/Tu%20Hai%20Unplugged-(Mr-Jatt.com).mp3" },
            new Photo { mPhotoID = Resource.Drawable.m2,imageUrl="https://static.blugaa.com/thumbs/100_100/jevai.jpg",
                        mCaption = "Tu Hi Hai",mArtist="Amit Trivedi , Ali Zafar",songUrl="http://dl.jatt.link/lq.jatt.link/cdn8/7d62adb19ee142aef0131c55d100114e/qwizv/Tu%20Hi%20Hai-(Mr-Jatt.com).mp3" },
            new Photo { mPhotoID = Resource.Drawable.m3,imageUrl="https://static.blugaa.com/thumbs/100_100/rkmai.jpg",
                        mCaption = "Tamma Tamma Again (Badrinath Ki Dulhania)",mArtist="Anuradha Paudwal , Bappi Lahiri , Badshah",songUrl="http://dl.jatt.link/lq.jatt.link/cdn8/2946545fc1b44592e6c45249e8d2e037/jfizv/Tamma%20Tamma%20Again-(Mr-Jatt.com).mp3" },
            new Photo { mPhotoID = Resource.Drawable.m4,imageUrl="https://static.blugaa.com/thumbs/100_100/icmai.jpg",
                        mCaption = "Mann Bawraa",mArtist="Varun Sinha",songUrl="http://dl.jatt.link/lq.jatt.link/cdn8/14a2fb18e59f81b1ef05fcbc1470d773/grpzv/Mann%20Bawraa-(Mr-Jatt.com).mp3" },
            new Photo { mPhotoID = Resource.Drawable.barish,imageUrl="https://static.blugaa.com/thumbs/100_100/wgiai.jpg",
                        mCaption = "Enna Sona (Remix)",mArtist="Dj Rishabh",songUrl="http://dl.jatt.link/lq.jatt.link/cdn8/b2be6b2a175775a2d94e3f60137126ab/xwlzv/Enna%20Sona%20%20Remix%20-(Mr-Jatt.com).mp3" },
            new Photo { mPhotoID = Resource.Drawable.m2,imageUrl="http://songsmp3.co/assets/images/1/96011-Screenshot_3.jpg",
                        mCaption = "Ik Vaari Aa (Raabta)",mArtist="Ik Vaari Aa (Raabta)",songUrl="http://themp3songs.com/48/283524/Ik%20Vaari%20Aa%20(Raabta)%20(TheMp3Songs.Com).mp3" }
            };

        // Array of photos that make up the album:
        private Photo[] mPhotos;

        // Random number generator for shuffling the photos:
        Random mRandom;

        // Create an instance copy of the built-in photo list and
        // create the random number generator:
        public PhotoAlbum()
        {
            mPhotos = mBuiltInPhotos;
            mRandom = new Random();
        }

        // Return the number of photos in the photo album:
        public int NumPhotos
        {
            get { return mPhotos.Length; }
        }

        // Indexer (read only) for accessing a photo:
        public Photo this[int i]
        {
            get { return mPhotos[i]; }
        }

        // Pick a random photo and swap it with the top:
        public int RandomSwap()
        {
            // Save the photo at the top:
            Photo tmpPhoto = mPhotos[0];

            // Generate a next random index between 1 and 
            // Length (noninclusive):
            int rnd = mRandom.Next(1, mPhotos.Length);

            // Exchange top photo with randomly-chosen photo:
            mPhotos[0] = mPhotos[rnd];
            mPhotos[rnd] = tmpPhoto;

            // Return the index of which photo was swapped with the top:
            return rnd;
        }

        // Shuffle the order of the photos:
        public void Shuffle()
        {
            // Use the Fisher-Yates shuffle algorithm:
            for (int idx = 0; idx < mPhotos.Length; ++idx)
            {
                // Save the photo at idx:
                Photo tmpPhoto = mPhotos[idx];

                // Generate a next random index between idx (inclusive) and 
                // Length (noninclusive):
                int rnd = mRandom.Next(idx, mPhotos.Length);

                // Exchange photo at idx with randomly-chosen (later) photo:
                mPhotos[idx] = mPhotos[rnd];
                mPhotos[rnd] = tmpPhoto;
            }
        }
    }
}