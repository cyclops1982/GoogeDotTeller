# GoogeDotTeller

dotnet core version of https://github.com/berthubert/googerteller

## Notes:

1. You'll need to use `git clone --recursive` as we are using the SharpPcap as a submodule.
2. You'll need a support pcap library, for windows that's [npcap](https://npcap.com/#download)
3. It currently does a `console.beep()` which likely doesn't work in many scenario's.
