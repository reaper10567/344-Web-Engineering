# 344-Web-Engineering

## Requirements
* [Visual Studio Community 2015](https://www.visualstudio.com/products/visual-studio-community-vs)
* [ASP.NET 5 Beta 7](http://www.microsoft.com/en-us/download/details.aspx?id=48738&fa43d42b-25b5-4a42-fe9b-1634f450f5ee=True) (Separate install since VS2015 does not come with the latest)
* [Node.js](https://nodejs.org/en/download/)
* Gulp `npm install -g gulp`
* Bower `npm install -g bower`

## Development Setup
> On Windows, using Git Bash as your command prompt/terminal is recommended.

1. Clone the repository.

    ```
    git clone https://github.com/reaper10567/344-Web-Engineering.git
    ```
2. Opening the project file `SE344.sln` in VS will automatically install server-side dependencies.
3. Install client-side dependencies.

    ```
    cd src/SE344
    npm install && bower install
    ```
4. Hack away.

## How To

### Automatically recompile SASS files during development

From the same folder that `gulpfile.js` is in, which should be `src/SE344`, run the following command:
```
gulp watch
```
