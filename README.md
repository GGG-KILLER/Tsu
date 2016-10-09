# GUtils.NET
A general utilities library I made from things I use frequently in all of my projects. Not all of it is made by me, in fact currently most of it is taken from stackoverflow and other places on the internet.

I decided to release this because some of my projects will be using this and I thought I should also make this open source.

# Requirements
- Jint
- Microsoft.Extensions.FileSystemGlobbing

# Features
- Heavily searched globbing file searching;
- Fast file copying (with configurable buffer size);
- Multithread form operations extension (just use .InvokeEx<T> on any of your form controls); ([StackOverflow](http://stackoverflow.com/a/711419/2671392))
- CloudFlare challenge solver (goes through the challenge page and returns you a webclient with access to the website); ([StackOverflow](http://stackoverflow.com/a/32426051/2671392))
- Relative path processing (get a path relative to another); ([StackOverflow](http://stackoverflow.com/a/6194678/2671392))
- Progress bar with progress text inside it; ([StackOverflow](http://stackoverflow.com/a/29175656))
- ToolTip Mangager (automatically show a tooltip with the text, icon and caption you configure when the user hovers a control);
- Folder Select Dialog on vista-style ([Bill at lyquidity.com](http://www.lyquidity.com/devblog/?p=136))
- Optimized bitmap drawing (way faster than .GetPixel);
- [WIP + Dropped] Bitmap drawing functions (squares, circles, etc.);
- PointD class (Point class using the Double type);
- SizeD class (Size class using the Double type);
- Explorer.exe utility class (open explorer on folder, open on folder with a file selected, open file with associated program).

# License

I feel it would be unfair to license other's code so just link to this page when you do use this. But for legal measures, I'll be using MIT Licence:

https://gggkiller.mit-license.org/