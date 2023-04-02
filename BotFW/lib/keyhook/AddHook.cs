using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace BotFW.lib.keyhook
{
    public struct keysAndCallback
    {
        public List<Key> Hotkey;
        public Action callback;
    }    
    public static class AddHook_i
    {
        private static List<AddHook> hookingdata = new List<AddHook>();
        public static void AddHook(Key key, Action callbak, bool suppres = false)
        {
            hookingdata.Add(new AddHook(key, callbak, suppres));
        }
        public static void AddHook(List<Key> key, Action callbak, bool suppres = false)
        {
            hookingdata.Add(new AddHook(key, callbak, suppres));
        }
    }
    public class AddHook
    {
        public AddHook(Key key, Action callbak, bool suppres = false)
        {
            Add_hook(key, callbak, suppres);
            Hook_en();
        }
        public AddHook(List<Key> key, Action callbak, bool suppres = false)
        {
            Add_hook(key, callbak, suppres);
            Hook_en();
        }

        private List<keysAndCallback> keysAndCallback_to_work = new List<keysAndCallback>();
        private Dictionary<Key, bool> keyFlags = new Dictionary<Key, bool>();
        private List<Key> keysToSuppres = new List<Key>();
        private void Add_hook(Key key, Action callbak, bool suppres = false)
        {
            //keysAndCallback_to_work = new keysAndCallback { callback = callbak, Hotkey = new List<Key> { key } };
            keysAndCallback_to_work.Add(new keysAndCallback { callback = callbak, Hotkey = new List<Key> { key } });
            keyFlags.Add(key, false);

            if (suppres)
                keysToSuppres.Add(key);
        }
        private void Add_hook(List<Key> key, Action callbak, bool suppres = false)
        {
            keysAndCallback_to_work.Add(new keysAndCallback { callback = callbak, Hotkey = key });
            foreach (var k in key)
            {
                keyFlags.Add(k, false);
                if (suppres)
                    keysToSuppres.Add(k);
            }
        }

        public void Hook_en()
        {
            Thread Newthread = new Thread(() => threadedThing());
            Newthread.Start();
        }
        private void threadedThing()
        {
            Hooking kb = new Hooking();
            kb.keysToSuppres = keysToSuppres;
            kb.KeyboardPressed += Kb_KeyboardPressed;
            System.Windows.Threading.Dispatcher.Run();

            kb.Dispose();
        }
        private void Kb_KeyboardPressed(object sender, KeyboardHookEventArgs e) // hook "r" // !!!!!!!
        {
            int okKeys = 0;
            foreach (var thing in keysAndCallback_to_work)
            {
                foreach (var k in thing.Hotkey)
                {
                    if (e.KeyPressType == Hooking.KeyPressType.KeyDown && e.InputEvent.Key == k)
                        keyFlags[k] = true;
                    else if (e.KeyPressType == Hooking.KeyPressType.KeyUp && e.InputEvent.Key == k)
                        keyFlags[k] = false;
                    else
                        keyFlags[k] = false;

                }
                foreach (var flag in keyFlags)
                    if (flag.Value)
                    {
                        okKeys++;
                    }
                if (okKeys == thing.Hotkey.Count)
                {
                    //Console.WriteLine("call bakc used!");
                    thing.callback();
                }    
                    
            }
        }
    }
}