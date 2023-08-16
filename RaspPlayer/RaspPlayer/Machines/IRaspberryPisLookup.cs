namespace RaspPlayer.RaspberryPis;

public interface IRaspberryPisLookup
{
    string GetNameByDeviceSerialNumberOrEmpty(string deviceSerialNumber);
}