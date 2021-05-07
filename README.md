# H-Xamarin-Forms-Simple-Hybrid-Engine
 Simple Hybrid Engine on Xamarin Forms (XFHybrid). A Simple HTTP Wrapper runs on Xamarin.Forms to run a HTML/JS web inside an App as an APP (pfftt). It basically works like Apache Cordova but this XFHybrid is not a specific platform base. Which means the `www` folder is not in "Android" or "iOS" project, it is inside the Xamarin.Forms project. This project is still under work in progress so there's not plugin yet for this project, but it enough to run data entry project with small features for media (camera and mic).
 
 # Why XFHybrid rather than Cordova?
 I have few apps run on Cordova, but I stopped since the Xamarin.Forms now getting very much stable. Why? Because it seems Xamarin.Forms has its advantage on security matter as its a "Renderrer to Native" rather than "Hybrid on top of native". This means anything run on Xamarin.Forms, it will be rendered and run as native, so it quite hard to reverse engineer the app.
 
 I made a few test on Cordova, simply extracting the `.apk` file, I can access the `www` folder with the whole assets includes other files like local certificates and others. So I think, why not put the `www` folder inside a renderer project like Xamarin.Forms, so here it is, XFHybrid. I tried run few test (this XFHybrid), the app quite big and a bit slow on start up, but I tried to reverse engineer the app, and I can't find the `www`. Why? Because the `www` is treated as Embedded Recource which is it already inside the compiled code.
 
 # The Cons
All Xamarin developer knows that Xamarin.Form is slow on startup and the `apk` file generate is like 3 times bigger than normal. I ran this empty project with bootstrap css, jquery, popper and single html file, it reaches 40mb! And it took around 5-10 second to startup. 

# What inside it?
Inside the `Solution Explorer`, there you will see the the Xamarin.Forms project named `HXFSimpleHybrid` and inside it you will find several folders as below:

1. `WWW` directory - The web folder which is the HTTP Engine will run as a default folder. (Well, for now, you can't change the folder the name nor change the folder)
2. `Core` directory - This is where the Hybrid Engine works behind the scene.  
- `HHttpServer` class - It contains the  which the core file in start/stop and routing the server. (include as an API to allow JS to access the SQLite database)
- `DB` class - It the core engine to run the SQLite Database. (still in dev - 7-May-2021)
- `Input` class - To read the HTTP request from the webview.
3. `Tables` directory - Contains all tables definition. (still in dev - 7-May-2021)
- `TableRoute` class - It routing the request from the `HHttpServer` request to deisgnated table definition. (still in dev - 7-May-2021)

# How to use?
Simple, just clone this project, and open it on visual studio. Make sure you have Visual Studio with Xamarin installed.

1. Please all your code or start develop you HTML / JS / CSS application inside the `WWW` directory.
2. Right to all your file > Properties > Build Action ~ set it to `Embedded Resource`. You have to do it the same thing to the other folder inside `WWW` folder. (it quite hard if you have a lot of sub folders ~ I will think how to make it simple)
3. By default, this project has set the Android Application to allow `cleartext` (non ssl web) to be accessed. So make sure you set the same thing if you create a new project.
4. Run ~~~~

# Future Plan
I will be developing the other feature, maybe integrating it with the native API and ensure its security at the highest security rank between others Hybrid Engine. LEL.

heryintelt@gmail.com
