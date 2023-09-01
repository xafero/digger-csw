﻿using System.Threading.Tasks;

namespace DiggerAPI
{
	public interface IDigger
	{
		IPc GetPc();

		bool KeyDown(int key);

		bool KeyUp(int key);

		Task RunAsync();
	}
}