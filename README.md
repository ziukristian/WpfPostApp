# WpfPostApp
This is a code assignment for an interview process, enjoy!

## Requirements
- Create a WPF application
- Fetch 100 posts using a public API
- Render each post in a separate square
- Put each square into a 10x10 grid
- Make it so, when a square is clicked, ID and User ID alternate their own rendering on the square

## How to run
Running it is quite simple since I've implemented a workflow to build and zip the exe file on release

- Simply go to the [code tab](https://github.com/ziukristian/WpfPostApp)
- On the right you should see the latest release
- By clicking on it you'll find both the zipped EXE file and the source code, download whichever you need
- Extract the EXE file
- Double-click on it to run it
- And that's it!
> [!WARNING]
> The released EXE is built for **Windows x64**, to run it using anything else you'll need to build it with the right configuration

## Considerations
- With this being my first WPF app, I did have some truble with organising the project for MVVM. Countless guides later I decided to go for the most common one I saw. 
- I considered implementing my own RelayCommand because that's what I saw most people do, but decided to use [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) which is what Microsoft also uses
- I started the project with the idea of making my own styles but rewriting the component templates sounded miserable, thus I looked for some premade styles and went for [ModernWPF](https://github.com/Kinnara/ModernWpf) which is not bad at all
- I took some liberties to make it more scalable, things such as making the grid sizes dynamic based on collection length and making a post service interface so it's not limited to the standard API
- There's not a lot of comments because I think we'll just be able to talk about it face to face
- I spent a lot of time deciding on a palette, I think it came out OK

## Assignment questions
### &#x1F534; In C# there are several ways to make code run in multiple threads. To make things easier, the await keyword was introduced; what does this do?
- The await keyword is used to maintain synchronous logic inside asynchronous methods. Let's say I'm firing an async method to receive some data and manipulate it without locking the main thread, inside the method I can awayt the call to receive the data so I can then manipulate it and return it to the main thread.

### &#x1F534; If you make http requests to a remote API directly from a UI component, the UI will freeze for a while, how can you use await to avoid this and how does this work?
- I can make the API call an async method, then call it and forget about it. In the method itself I can await the data, assign it to a rendered property and then signal the UI for a refresh.

### &#x1F534; Imagine that you have to process a large unsorted CSV file with two columns: ProductId (int) and AvailableIn (ISO2 String, e.g. "US", "NL"). The goal is to group the file sorted by ProductId together with a list where the product is available. Example: 1, "DE" 2, "NL" 1, "US" 3, "US" Becomes: 1 -> ["DE", "US"] 2 -> ["NL"] 3 -> ["US"]
### 1 - How would you do this using LINQ syntax (write a short example)?

```
var result = products
    .GroupBy(s => s.ProductId)
    .Select(g => new
    {
        ProductId = g.Key,
        AvailableIn = g.Select(i => i.AvailableIn).Distinct().ToList(),
    })
    .ToList();
```
also found this one which looks even better
```
var result = products
    .GroupBy(s => s.ProductId)
    .Select(g => new
    {
        ProductId = g.Key,
        AvailableIn = new HashSet<string>(g.Select(i => i.AvailableIn)),
    })
    .ToList();
```
### 2 - The program crashes with an OutOfMemoryError after processing approx. 80%. What would you do to succeed?
I would split the data up, work on a single batch at a time while saving results to a database that supports arrays.

### &#x1F534; In C# there is an interface IDisposable.
### 1 - Give an example of where and why to implement this interface.
First thing that comes to mind is a class that manipulates files. It would be a good way to make sure any opened file is closed asap and ready to be accessed by others, even if there are exceptions.
### 2 - We can use disposable objects in a using block. What is the purpose of doing this?
So when we exit the block the object's implementation of Dispose can be called. We don't even need a block in .net 8 since using disposes the target at scope end.

### &#x1F534; When a user logs in on our API, a JWT token is issued and our Outlook plugin uses this token for every request for authentication. Here's an example of such a token:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkplcmVteSIsImFkbWluIjpmYWxzZX0.BgcLOiwBvyuisQk9yWW0q0ZSc MyIHNmDctw12-meCHU
```
### 1 - Why and when is it (or isn't it) safe to use this?
JWT tokens are safe as long as they are hashed with a good server side secret, there's not too much data in the body (since it's only encoded and not encrypted) and it's lifecycle is not too long.

If everything has been implemented correctly, the only thing you have to worry about is the client getting compromised and having the token stolen. That would make it impossible to distinguish a bad actor from the client itself.
Sending the token only on secure connections and saving an eventual refresh tokens in an httpOnly cookie (if we were talking about browsers) would make it safer.

The token seems pretty safe since it's hashed and there's only a single claim, a username and a subject

## Conclusion
Well, thank you for the time you've dedicated into reading all of this. Bye

