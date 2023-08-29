namespace DiggerClassic.Core
{
	internal class Input
	{
		internal bool leftpressed;
		internal bool rightpressed;
		internal bool uppressed;
		internal bool downpressed;
		internal bool f1pressed;
		internal bool firepressed;
		internal bool minuspressed;
		internal bool pluspressed;
		internal bool f10pressed;
		internal bool escape;
		internal int keypressed;
		internal int akeypressed;
		int dynamicdir = -1;
		int staticdir = -1;
		int joyx = 0;
		int joyy = 0;
		bool joybut1 = false;
		bool joybut2 = false;
		int keydir;
		int jleftthresh = 0;
		int jupthresh = 0;
		int jrightthresh = 0;
		int jdownthresh = 0;
		int joyanax = 0;
		int joyanay = 0;
		internal bool firepflag;
		bool joyflag;

		Digger dig;

		internal Input(Digger d)
		{
			dig = d;
		}

		internal void checkkeyb()
		{
			if (pluspressed)
			{
				if (dig.frametime > Digger.MIN_RATE)
					dig.frametime -= 5;
			}
			if (minuspressed)
			{
				if (dig.frametime < Digger.MAX_RATE)
					dig.frametime += 5;
			}
			if (f10pressed)
				escape = true;
		}

		internal void detectjoy()
		{
			joyflag = false;
			staticdir = dynamicdir = -1;
		}

		internal int getasciikey(int make)
		{
			int k;
			if ((make == ' ') || ((make >= 'a') && (make <= 'z')) || ((make >= '0') && (make <= '9')))
				return make;
			return 0;
		}

		internal int getdir()
		{
			var bp2 = keydir;
			return bp2;
		}

		internal void initkeyb()
		{
		}

		void Key_downpressed()
		{
			downpressed = true;
			dynamicdir = staticdir = 6;
		}

		void Key_downreleased()
		{
			downpressed = false;
			if (dynamicdir == 6)
				setdirec();
		}

		void Key_f1pressed()
		{
			firepressed = true;
			f1pressed = true;
		}

		void Key_f1released()
		{
			f1pressed = false;
		}

		void Key_leftpressed()
		{
			leftpressed = true;
			dynamicdir = staticdir = 4;
		}

		void Key_leftreleased()
		{
			leftpressed = false;
			if (dynamicdir == 4)
				setdirec();
		}

		void Key_rightpressed()
		{
			rightpressed = true;
			dynamicdir = staticdir = 0;
		}

		void Key_rightreleased()
		{
			rightpressed = false;
			if (dynamicdir == 0)
				setdirec();
		}

		void Key_uppressed()
		{
			uppressed = true;
			dynamicdir = staticdir = 2;
		}

		void Key_upreleased()
		{
			uppressed = false;
			if (dynamicdir == 2)
				setdirec();
		}

		internal void processkey(int key)
		{
			keypressed = key;
			if (key > 0x80)
				akeypressed = key & 0x7f;
			switch (key)
			{
				case 0x4b:
					Key_leftpressed();
					break;
				case 0xcb:
					Key_leftreleased();
					break;
				case 0x4d:
					Key_rightpressed();
					break;
				case 0xcd:
					Key_rightreleased();
					break;
				case 0x48:
					Key_uppressed();
					break;
				case 0xc8:
					Key_upreleased();
					break;
				case 0x50:
					Key_downpressed();
					break;
				case 0xd0:
					Key_downreleased();
					break;
				case 0x3b:
					Key_f1pressed();
					break;
				case 0xbb:
					Key_f1released();
					break;
				case 0x78:
					f10pressed = true;
					break;
				case 0xf8:
					f10pressed = false;
					break;
				case 0x2b:
					pluspressed = true;
					break;
				case 0xab:
					pluspressed = false;
					break;
				case 0x2d:
					minuspressed = true;
					break;
				case 0xad:
					minuspressed = false;
					break;
			}
		}

		internal void readdir()
		{
			keydir = staticdir;
			if (dynamicdir != -1)
				keydir = dynamicdir;
			staticdir = -1;
			if (f1pressed || firepressed)
				firepflag = true;
			else
				firepflag = false;
			firepressed = false;
		}

		void readjoy()
		{
		}

		void setdirec()
		{
			dynamicdir = -1;
			if (uppressed) dynamicdir = staticdir = 2;
			if (downpressed) dynamicdir = staticdir = 6;
			if (leftpressed) dynamicdir = staticdir = 4;
			if (rightpressed) dynamicdir = staticdir = 0;
		}

		internal bool teststart()
		{
			var startf = false;
			if (keypressed != 0 && (keypressed & 0x80) == 0 && keypressed != 27)
			{
				startf = true;
				joyflag = false;
				keypressed = 0;
			}
			if (!startf)
				return false;
			return true;
		}
	}
}