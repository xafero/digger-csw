using System.Threading;

namespace DiggerClassic.Core
{
	/// <summary>
	/// Sound has not been ported yet
	/// </summary>
	internal sealed class Sound
	{
		int wavetype = 0;
		int t2val = 0;
		int t0val = 0;
		int musvol = 0;
		int spkrmode = 0;
		int timerrate = 0x7d0;
		int timercount = 0;
		int pulsewidth = 1;
		internal int volume = 0;
		int timerclock = 0;
		internal bool soundflag = true;
		internal bool musicflag = true;
		bool sndflag = false;
		bool soundpausedflag = false;
		bool soundlevdoneflag = false;
		int nljpointer = 0;
		int nljnoteduration = 0;

		int[] newlevjingle =
		{
			0x8e8, 0x712, 0x5f2, 0x7f0, 0x6ac, 0x54c, 0x712, 0x5f2, 0x4b8, 0x474, 0x474
		}; // [11]

		bool soundfallflag = false;
		bool soundfallf = false;
		int soundfallvalue;
		int soundfalln = 0;
		bool soundbreakflag = false;
		int soundbreakduration = 0;
		int soundbreakvalue = 0;
		bool soundwobbleflag = false;
		int soundwobblen = 0;
		bool soundfireflag = false;
		int soundfirevalue;
		int soundfiren = 0;
		bool soundexplodeflag = false;
		int soundexplodevalue;
		int soundexplodeduration;
		bool soundbonusflag = false;
		int soundbonusn = 0;
		bool soundemflag = false;
		bool soundemeraldflag = false;
		int soundemeraldduration;
		int emerfreq;
		int soundemeraldn;
		bool soundgoldflag = false, soundgoldf = false;
		int soundgoldvalue1;
		int soundgoldvalue2;
		int soundgoldduration;
		bool soundeatmflag = false;
		int soundeatmvalue;
		int soundeatmduration;
		int soundeatmn;
		bool soundddieflag = false;
		int soundddien;
		int soundddievalue;
		bool sound1upflag = false;
		int sound1upduration = 0;
		bool musicplaying = false;
		int musicp = 0;
		int tuneno = 0;
		int noteduration = 0;
		int notevalue = 0;
		int musicmaxvol = 0;
		int musicattackrate = 0;
		int musicsustainlevel = 0;
		int musicdecayrate = 0;
		int musicnotewidth = 0;
		int musicreleaserate = 0;
		int musicstage = 0;
		int musicn = 0;

		bool soundt0flag = false;
		bool int8flag = false;

		Digger dig;

		internal Sound(Digger d)
		{
			dig = d;
		}

		internal void initsound()
		{
			wavetype = 2;
			t0val = 12000;
			musvol = 8;
			t2val = 40;
			soundt0flag = true;
			sndflag = true;
			spkrmode = 0;
			int8flag = false;
			setsoundt2();
			soundstop();
			startint8();
			timerrate = 0x4000;
		}

		internal void killsound()
		{
		}

		internal void music(int tune)
		{
			tuneno = tune;
			musicp = 0;
			noteduration = 0;
			switch (tune)
			{
				case 0:
					musicmaxvol = 50;
					musicattackrate = 20;
					musicsustainlevel = 20;
					musicdecayrate = 10;
					musicreleaserate = 4;
					break;
				case 1:
					musicmaxvol = 50;
					musicattackrate = 50;
					musicsustainlevel = 8;
					musicdecayrate = 15;
					musicreleaserate = 1;
					break;
				case 2:
					musicmaxvol = 50;
					musicattackrate = 50;
					musicsustainlevel = 25;
					musicdecayrate = 5;
					musicreleaserate = 1;
					break;
			}
			musicplaying = true;
			if (tune == 2)
				soundddieoff();
		}

		internal void musicoff()
		{
			musicplaying = false;
			musicp = 0;
		}

		void musicupdate()
		{
			if (!musicplaying)
				return;
			if (noteduration != 0)
				noteduration--;
			else
			{
				musicstage = musicn = 0;
				switch (tuneno)
				{
					case 0:
						musicnotewidth = noteduration - 3;
						musicp += 2;
						break;
					case 1:
						musicnotewidth = 12;
						musicp += 2;
						break;
					case 2:
						musicnotewidth = noteduration - 10;
						musicp += 2;
						break;
				}
			}
			musicn++;
			wavetype = 1;
			t0val = notevalue;
			if (musicn >= musicnotewidth)
				musicstage = 2;
			switch (musicstage)
			{
				case 0:
					if (musvol + musicattackrate >= musicmaxvol)
					{
						musicstage = 1;
						musvol = musicmaxvol;
						break;
					}
					musvol += musicattackrate;
					break;
				case 1:
					if (musvol - musicdecayrate <= musicsustainlevel)
					{
						musvol = musicsustainlevel;
						break;
					}
					musvol -= musicdecayrate;
					break;
				case 2:
					if (musvol - musicreleaserate <= 1)
					{
						musvol = 1;
						break;
					}
					musvol -= musicreleaserate;
					break;
			}
			if (musvol == 1)
				t0val = 0x7d00;
		}

		void s0fillbuffer()
		{
		}

		void s0killsound()
		{
			setsoundt2();
			stopint8();
		}

		void s0setupsound()
		{
			startint8();
		}

		void setsoundmode()
		{
			spkrmode = wavetype;
			if (!soundt0flag && sndflag)
			{
				soundt0flag = true;
			}
		}

		internal void setsoundt2()
		{
			if (soundt0flag)
			{
				spkrmode = 0;
				soundt0flag = false;
			}
		}

		void sett0()
		{
			if (sndflag)
			{
				if (t0val < 1000 && (wavetype == 1 || wavetype == 2))
					t0val = 1000;
				timerrate = t0val;
				if (musvol < 1)
					musvol = 1;
				if (musvol > 50)
					musvol = 50;
				pulsewidth = musvol * volume;
				setsoundmode();
			}
		}

		internal void sett2val(int t2v)
		{
		}

		internal void setupsound()
		{
		}

		internal void sound1up()
		{
			sound1upduration = 96;
			sound1upflag = true;
		}

		void sound1upoff()
		{
			sound1upflag = false;
		}

		void sound1upupdate()
		{
			if (sound1upflag)
			{
				if ((sound1upduration / 3) % 2 != 0)
					t2val = (sound1upduration << 2) + 600;
				sound1upduration--;
				if (sound1upduration < 1)
					sound1upflag = false;
			}
		}

		internal void soundbonus()
		{
			soundbonusflag = true;
		}

		internal void soundbonusoff()
		{
			soundbonusflag = false;
			soundbonusn = 0;
		}

		void soundbonusupdate()
		{
			if (soundbonusflag)
			{
				soundbonusn++;
				if (soundbonusn > 15)
					soundbonusn = 0;
				if (soundbonusn >= 0 && soundbonusn < 6)
					t2val = 0x4ce;
				if (soundbonusn >= 8 && soundbonusn < 14)
					t2val = 0x5e9;
			}
		}

		internal void soundbreak()
		{
			soundbreakduration = 3;
			if (soundbreakvalue < 15000)
				soundbreakvalue = 15000;
			soundbreakflag = true;
		}

		void soundbreakoff()
		{
			soundbreakflag = false;
		}

		void soundbreakupdate()
		{
			if (soundbreakflag)
				if (soundbreakduration != 0)
				{
					soundbreakduration--;
					t2val = soundbreakvalue;
				}
				else
					soundbreakflag = false;
		}

		internal void soundddie()
		{
			soundddien = 0;
			soundddievalue = 20000;
			soundddieflag = true;
		}

		void soundddieoff()
		{
			soundddieflag = false;
		}

		void soundddieupdate()
		{
			if (soundddieflag)
			{
				soundddien++;
				if (soundddien == 1)
					musicoff();
				if (soundddien >= 1 && soundddien <= 10)
					soundddievalue = 20000 - soundddien * 1000;
				if (soundddien > 10)
					soundddievalue += 500;
				if (soundddievalue > 30000)
					soundddieoff();
				t2val = soundddievalue;
			}
		}

		internal void soundeatm()
		{
			soundeatmduration = 20;
			soundeatmn = 3;
			soundeatmvalue = 2000;
			soundeatmflag = true;
		}

		void soundeatmoff()
		{
			soundeatmflag = false;
		}

		void soundeatmupdate()
		{
			if (soundeatmflag)
				if (soundeatmn != 0)
				{
					if (soundeatmduration != 0)
					{
						if ((soundeatmduration % 4) == 1)
							t2val = soundeatmvalue;
						if ((soundeatmduration % 4) == 3)
							t2val = soundeatmvalue - (soundeatmvalue >> 4);
						soundeatmduration--;
						soundeatmvalue -= (soundeatmvalue >> 4);
					}
					else
					{
						soundeatmduration = 20;
						soundeatmn--;
						soundeatmvalue = 2000;
					}
				}
				else
					soundeatmflag = false;
		}

		internal void soundem()
		{
			soundemflag = true;
		}

		internal void soundemerald(int emocttime)
		{
			if (emocttime != 0)
			{
				switch (emerfreq)
				{
					case 0x8e8:
						emerfreq = 0x7f0;
						break;
					case 0x7f0:
						emerfreq = 0x712;
						break;
					case 0x712:
						emerfreq = 0x6ac;
						break;
					case 0x6ac:
						emerfreq = 0x5f2;
						break;
					case 0x5f2:
						emerfreq = 0x54c;
						break;
					case 0x54c:
						emerfreq = 0x4b8;
						break;
					case 0x4b8:
						emerfreq = 0x474;
						dig.Scores.scoreoctave();
						break;
					case 0x474:
						emerfreq = 0x8e8;
						break;
				}
			}
			else
				emerfreq = 0x8e8;
			soundemeraldduration = 7;
			soundemeraldn = 0;
			soundemeraldflag = true;
		}

		void soundemeraldoff()
		{
			soundemeraldflag = false;
		}

		void soundemeraldupdate()
		{
			if (soundemeraldflag)
				if (soundemeraldduration != 0)
				{
					if (soundemeraldn == 0 || soundemeraldn == 1)
						t2val = emerfreq;
					soundemeraldn++;
					if (soundemeraldn > 7)
					{
						soundemeraldn = 0;
						soundemeraldduration--;
					}
				}
				else
					soundemeraldoff();
		}

		void soundemoff()
		{
			soundemflag = false;
		}

		void soundemupdate()
		{
			if (soundemflag)
			{
				t2val = 1000;
				soundemoff();
			}
		}

		internal void soundexplode()
		{
			soundexplodevalue = 1500;
			soundexplodeduration = 10;
			soundexplodeflag = true;
			soundfireoff();
		}

		void soundexplodeoff()
		{
			soundexplodeflag = false;
		}

		void soundexplodeupdate()
		{
			if (soundexplodeflag)
				if (soundexplodeduration != 0)
				{
					soundexplodevalue = t2val = soundexplodevalue - (soundexplodevalue >> 3);
					soundexplodeduration--;
				}
				else
					soundexplodeflag = false;
		}

		internal void soundfall()
		{
			soundfallvalue = 1000;
			soundfallflag = true;
		}

		internal void soundfalloff()
		{
			soundfallflag = false;
			soundfalln = 0;
		}

		void soundfallupdate()
		{
			if (soundfallflag)
				if (soundfalln < 1)
				{
					soundfalln++;
					if (soundfallf)
						t2val = soundfallvalue;
				}
				else
				{
					soundfalln = 0;
					if (soundfallf)
					{
						soundfallvalue += 50;
						soundfallf = false;
					}
					else
						soundfallf = true;
				}
		}

		internal void soundfire()
		{
			soundfirevalue = 500;
			soundfireflag = true;
		}

		internal void soundfireoff()
		{
			soundfireflag = false;
			soundfiren = 0;
		}

		void soundfireupdate()
		{
			if (soundfireflag)
			{
				if (soundfiren == 1)
				{
					soundfiren = 0;
					soundfirevalue += soundfirevalue / 55;
					t2val = soundfirevalue + dig.Main.randno(soundfirevalue >> 3);
					if (soundfirevalue > 30000)
						soundfireoff();
				}
				else
					soundfiren++;
			}
		}

		internal void soundgold()
		{
			soundgoldvalue1 = 500;
			soundgoldvalue2 = 4000;
			soundgoldduration = 30;
			soundgoldf = false;
			soundgoldflag = true;
		}

		void soundgoldoff()
		{
			soundgoldflag = false;
		}

		void soundgoldupdate()
		{
			if (soundgoldflag)
			{
				if (soundgoldduration != 0)
					soundgoldduration--;
				else
					soundgoldflag = false;
				if (soundgoldf)
				{
					soundgoldf = false;
					t2val = soundgoldvalue1;
				}
				else
				{
					soundgoldf = true;
					t2val = soundgoldvalue2;
				}
				soundgoldvalue1 += (soundgoldvalue1 >> 4);
				soundgoldvalue2 -= (soundgoldvalue2 >> 4);
			}
		}

		void soundint()
		{
			timerclock++;
			if (soundflag && !sndflag)
				sndflag = musicflag = true;
			if (!soundflag && sndflag)
			{
				sndflag = false;
				setsoundt2();
			}
			if (sndflag && !soundpausedflag)
			{
				t0val = 0x7d00;
				t2val = 40;
				if (musicflag)
					musicupdate();
				soundemeraldupdate();
				soundwobbleupdate();
				soundddieupdate();
				soundbreakupdate();
				soundgoldupdate();
				soundemupdate();
				soundexplodeupdate();
				soundfireupdate();
				soundeatmupdate();
				soundfallupdate();
				sound1upupdate();
				soundbonusupdate();
				if (t0val == 0x7d00 || t2val != 40)
					setsoundt2();
				else
				{
					setsoundmode();
					sett0();
				}
				sett2val(t2val);
			}
		}

		internal void soundlevdone()
		{
			try
			{
				// TODO Remove sleep?
				Thread.Sleep(1000);
			}
			catch (System.Exception e)
			{
			}
		}

		void soundlevdoneoff()
		{
			soundlevdoneflag = soundpausedflag = false;
		}

		void soundlevdoneupdate()
		{
			if (sndflag)
			{
				if (nljpointer < 11)
					t2val = newlevjingle[nljpointer];
				t0val = t2val + 35;
				musvol = 50;
				setsoundmode();
				sett0();
				sett2val(t2val);
				if (nljnoteduration > 0)
					nljnoteduration--;
				else
				{
					nljnoteduration = 20;
					nljpointer++;
					if (nljpointer > 10)
						soundlevdoneoff();
				}
			}
			else
			{
				soundlevdoneflag = false;
			}
		}

		void soundoff()
		{
		}

		internal void soundpause()
		{
			soundpausedflag = true;
		}

		internal void soundpauseoff()
		{
			soundpausedflag = false;
		}

		internal void soundstop()
		{
			soundfalloff();
			soundwobbleoff();
			soundfireoff();
			musicoff();
			soundbonusoff();
			soundexplodeoff();
			soundbreakoff();
			soundemoff();
			soundemeraldoff();
			soundgoldoff();
			soundeatmoff();
			soundddieoff();
			sound1upoff();
		}

		internal void soundwobble()
		{
			soundwobbleflag = true;
		}

		internal void soundwobbleoff()
		{
			soundwobbleflag = false;
			soundwobblen = 0;
		}

		void soundwobbleupdate()
		{
			if (soundwobbleflag)
			{
				soundwobblen++;
				if (soundwobblen > 63)
					soundwobblen = 0;
				switch (soundwobblen)
				{
					case 0:
						t2val = 0x7d0;
						break;
					case 16:
					case 48:
						t2val = 0x9c4;
						break;
					case 32:
						t2val = 0xbb8;
						break;
				}
			}
		}

		void startint8()
		{
			if (!int8flag)
			{
				timerrate = 0x4000;
				int8flag = true;
			}
		}

		void stopint8()
		{
			if (int8flag)
			{
				int8flag = false;
			}
			sett2val(40);
		}
	}
}