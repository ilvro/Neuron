from sys import argv
from webbrowser import open as openUrl;

def launchClient(ticket, trackerID, placeID, *args):
    print(ticket, trackerID, placeID);
    openUrl(
                f"roblox-player:1+launchmode:play+gameinfo:{ticket}+launchtime:1668477385172+"\
                f"placelauncherurl:https%3A%2F%2Fassetgame.roblox.com%2Fgame%2FPlaceLauncher.ashx%3F"\
                f"request%3DRequestGame%26browserTrackerId%3D{trackerID}%26placeId%3D{placeID}%26"\
                f"isPlayTogetherGame%3Dfalse+browsertrackerid:{trackerID}+robloxLocale:en_us+gameLocale:en_us"
        );

launchClient(*argv[1:])