using Android.App;
using Android.Widget;
using Android.OS;
using Android.Text.Method;
using Cheesebaron.SlidingUpPanel;
using Android.Util;
using Android.Support.V7.App;
using Android.Media;
using static Android.Media.MediaPlayer;
using System;
using Android.Support.V7.Widget;
using Com.Bumptech.Glide;

namespace KonkaniMusic
{
    [Activity(Label = "Konkani Music", Icon = "@drawable/music",
        Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        public SeekBar seekbar;
        MediaPlayer mp;
        string mp3;
        bool playing = false;
        ProgressDialog p;        
        private const string SavedStateActionBarHidden = "saved_state_action_bar_hidden";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            var layout = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
            playbt = FindViewById<ImageButton>(Resource.Id.bt_play);
            playbtn = FindViewById<ImageButton>(Resource.Id.play);
            queuebtn = FindViewById<ImageView>(Resource.Id.queue);
            albumArt= FindViewById<ImageView>(Resource.Id.album_art);
            txtSongName = FindViewById<TextView>(Resource.Id.SongNameText);
            txtArtistName = FindViewById<TextView>(Resource.Id.ArtistNameText);
            p = new ProgressDialog(this);
            p.SetMessage("Loading song....");
            p.SetCancelable(false);

            var maintext = FindViewById<TextView>(Resource.Id.mainFeatured);

            maintext.Text = " Featured songs: ";
            mPhotoAlbum = new PhotoAlbum();
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewFeatured);
            mLayoutManager = new LinearLayoutManager(this, 0, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new PhotoAlbumAdapter(mPhotoAlbum,this);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            maintext = FindViewById<TextView>(Resource.Id.mainNew);

            maintext.Text = " New songs: ";
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewNew);
            mLayoutManager = new LinearLayoutManager(this, 0, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new PhotoAlbumAdapter(mPhotoAlbum,this);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            maintext = FindViewById<TextView>(Resource.Id.mainDownloaded);

            maintext.Text = " Downloaded songs: ";
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewDownloaded);
            mLayoutManager = new LinearLayoutManager(this, 0, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new PhotoAlbumAdapter(mPhotoAlbum,this);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            seekbar = FindViewById<SeekBar>(Resource.Id.songProgressBar);


            mp = new MediaPlayer();
            mp.BufferingUpdate += updateSeekbar;
            mp.SetAudioStreamType(Stream.Music);
            mp.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);

            mp.Prepared += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("called prepare function :)" + mp.Duration);
                p.Hide();
                seekbar.Max = mp.Duration;
                //seekbar_top.Max = mp.Duration;
                mp.Start();
                System.Diagnostics.Debug.WriteLine("called prepare function here only:)" + mp.Duration);
                updateProgressBar();
            };
            mp.Completion += (sender, args) => StopMusic();            
            playbtn.Click += (s, e) => {
                System.Diagnostics.Debug.WriteLine("play clicked");
                System.Diagnostics.Debug.WriteLine(mp3);

                if (!playing)
                {
                    PlayMusic(mp3);
                    playing = true;
                    playbtn.SetImageResource(Resource.Mipmap.pause);
                    playbt.SetImageResource(Resource.Mipmap.pause);

                }
                else
                {
                    PauseMusic();
                    playing = false;
                    playbtn.SetImageResource(Resource.Mipmap.playy);
                    playbt.SetImageResource(Resource.Mipmap.playy);

                }
                Toast.MakeText(ApplicationContext, "play clicked", ToastLength.Short).Show();
            };

            queuebtn.Click += (s, e) => {
                System.Diagnostics.Debug.WriteLine("queue clicked");
                Toast.MakeText(ApplicationContext, "queue clicked", ToastLength.Short).Show();
            };

            playbt.Click += (s, e) => {
                System.Diagnostics.Debug.WriteLine("bt_play clicked");
                if (!playing)
                {
                    PlayMusic(mp3);
                    playing = true;
                    playbtn.SetImageResource(Resource.Mipmap.pause);
                    playbt.SetImageResource(Resource.Mipmap.pause);
                }
                else
                {
                    PauseMusic();
                    playing = false;
                    playbtn.SetImageResource(Resource.Mipmap.playy);
                    playbt.SetImageResource(Resource.Mipmap.playy);
                }
                Toast.MakeText(ApplicationContext, "bt_play clicked", ToastLength.Short).Show();
            };            
            mp.Error += (sender, args) =>
            {
                p.Hide();                
                StopMusic();
                mp.Reset();
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Network problem!");
                alert.SetMessage("Try again..");
                alert.SetPositiveButton("Retry", (senderAlert, e) => {
                    PlayMusic(mp3, true);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, e) => {
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };

            seekbar.ProgressChanged += Seekbar_ProgressChanged;
            seekbar.StartTrackingTouch += Seekbar_StartTrackingTouch;
            seekbar.StopTrackingTouch += Seekbar_StopTrackingTouch;

            txtSongName.Text = mPhotoAlbum[0].mCaption.ToString();
            txtArtistName.Text = mPhotoAlbum[0].mArtist.ToString();
            mp3 = mPhotoAlbum[0].songUrl;
            Glide.With(this)
            .Load(mPhotoAlbum[0].imageUrl)
            .Placeholder(Resource.Mipmap.queue)
            .Into(queuebtn);
            Glide.With(this)
            .Load(mPhotoAlbum[0].imageUrl)
            .Placeholder(Resource.Mipmap.music)
            .Into(albumArt);

            layout.Click += (s, e) =>
              {
                  Toast.MakeText(ApplicationContext, "layout clicked", ToastLength.Short).Show();
              };
            layout.AnchorPoint = 0.3f;
            layout.PanelExpanded += (s, e) => { System.Diagnostics.Debug.WriteLine("PanelExpanded");
                playbtn.Visibility = Android.Views.ViewStates.Invisible;
                queuebtn.Visibility = Android.Views.ViewStates.Invisible;
                //txtArtistName.Visibility = Android.Views.ViewStates.Invisible;
            };
            layout.PanelCollapsed += (s, e) => { System.Diagnostics.Debug.WriteLine("PanelCollapsed");
                playbtn.Visibility = Android.Views.ViewStates.Visible;
                queuebtn.Visibility = Android.Views.ViewStates.Visible;
                //txtArtistName.Visibility = Android.Views.ViewStates.Visible;
            };
            layout.PanelAnchored += (s, e) => System.Diagnostics.Debug.WriteLine("PanelAnchored");
            layout.PanelSlide += (s, e) =>
            {
                if (e.SlideOffset < 0.2)
                {
                    if (SupportActionBar.IsShowing)
                    {
                        SupportActionBar.Hide();
                    }
                }
                else
                {
                    if (!SupportActionBar.IsShowing)
                        SupportActionBar.Show();
                }
            };

            var actionBarHidden = bundle != null &&
                                  bundle.GetBoolean(SavedStateActionBarHidden, false);
            if (actionBarHidden)
                SupportActionBar.Hide();
        }

        private void OnItemClick(object sender, int e)
        {
            var recycler = sender as PhotoAlbumAdapter;
            Toast.MakeText(this, recycler.mPhotoAlbum[e].mCaption, ToastLength.Short).Show();
            txtSongName.Text = recycler.mPhotoAlbum[e].mCaption.ToString();
            txtArtistName.Text =recycler.mPhotoAlbum[e].mArtist.ToString();
            mp3 = recycler.mPhotoAlbum[e].songUrl;
            Glide.With(this)
            .Load(recycler.mPhotoAlbum[e].imageUrl)
            .Placeholder(Resource.Mipmap.queue)
            .Into(queuebtn);
            Glide.With(this)
            .Load(recycler.mPhotoAlbum[e].imageUrl)
            .Placeholder(Resource.Mipmap.music)
            .Into(albumArt);
            PlayMusic(mp3,true);
            playing = true;
            playbtn.SetImageResource(Resource.Mipmap.pause);
            playbt.SetImageResource(Resource.Mipmap.pause);
        }

        //seekbar events
        private void updateSeekbar(object sender, BufferingUpdateEventArgs e)
        {
            var x = (MediaPlayer)sender;
            System.Diagnostics.Debug.WriteLine("seeking to : " + x.CurrentPosition + " from " + seekbar.Max);
            seekbar.SecondaryProgress = (e.Percent * mp.Duration) / 100;
        }
        private void Seekbar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {

        }
        private void Seekbar_StartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e)
        {
            mHandler.RemoveCallbacks(updateTimeTask);
        }
        private void Seekbar_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            if (mp != null)
                if (mp.IsPlaying)
                    mp.SeekTo(seekbar.Progress);
        }
        private void updateProgressBar()
        {
            mHandler.PostDelayed(updateTimeTask, 1000);
        }
        private void updateTimeTask()
        {
            System.Diagnostics.Debug.WriteLine("My timer call ::  seeking the bar to position");
            if (mp != null)
            {
                if (mp.IsPlaying)
                    if (mp.CurrentPosition < seekbar.Max)
                    {
                        seekbar.SetProgress(mp.CurrentPosition, true);
                        //seekbar_top.SetProgress(mp.CurrentPosition, true);
                        mHandler.PostDelayed(updateTimeTask, 1000);
                    }
            }
        }

        Handler mHandler = new Handler();
        private int length = 0;
        private PhotoAlbum mPhotoAlbum;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private PhotoAlbumAdapter mAdapter;
        private TextView txtSongName;
        private TextView txtArtistName;
        private ImageView queuebtn;
        private ImageButton playbt;
        private ImageButton playbtn;
        private ImageView albumArt;

        //music stop play pause methods

        private void StopMusic()
        {
            if (mp != null)
                if (mp.IsPlaying)
                    mp.Stop();
        }

        private void PauseMusic()
        {
            if (mp != null)
                if (mp.IsPlaying)
                {
                    mp.Pause();
                    length = mp.CurrentPosition;
                }
        }        
        private async void PlayMusic(string rmp3,bool newsong=false)
        {
            if (rmp3 == "")
                rmp3 = "http://cdn5.jatt.link/f7861b11c29524fa7c8035af8d4a3847/ssqgv/Ankhon%20Mein%20Teri-(Mr-Jatt.com).mp3";

            if (length != 0 && newsong==false)
            {
                mp.SeekTo(length);
                mp.Start();
                updateProgressBar();
            }
            else
            {
                mp.Reset();               
                p.Show();
                await mp.SetDataSourceAsync(ApplicationContext, Android.Net.Uri.Parse(rmp3));                
                mp.PrepareAsync();
                             
            }
        }

        public override void OnBackPressed()
        {
            SlidingUpPanelLayout ss = (SlidingUpPanelLayout)FindViewById(Resource.Id.sliding_layout);
            if (ss != null && (ss.IsExpanded || ss.IsAnchored))
            {
                ss.CollapsePane();                
            }
            //else
            //{
            //    base.OnBackPressed();
            //}
        }
    }    
}

