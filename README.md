# 1337-code-assignment

This is a solution of a code assignment from 1337.

The solution consists of a console app that traverses a web site and stores it on the local file system.

# Installation instructions
* Download the source code and use Visual Studio (with .net 6.0) to build the solution `WebTraverser.sln`

OR

* For windows, you can download the prebuilt one-file executable `WebTraverser.exe` located under **Releases** on GitHub

# How to run
* Open a command prompt and navigate to the folder with **WebTraverser.exe**
* Run WebTraverser.exe to traverse the default site (https://tretton1337.com) into a folder named **result** 
* To specify a different result folder, pass the folder name as first parameter like this:

  `WebTraverser.exe anotherFolder`
  
* To specify a different site to traverse, pass the site address as second parameter, like this:

  `WebTraverser.exe resultFolder https://myOtherSite.com`


# Code design details
Some thoughts regarding the implementation:
* The main program starts the traversing by invoking an async Run method, which starts traversing from the website root supplied
* Traversing is done by extracting href-links from the page, and follow those that are considered relevant. To make it simple, only links starting with `/` are considered relevant, since they refer to pages on same site.
* We make sure that every link is only traversed once, since otherwise we might easily get stuck in an infinity loop, since web pages often links to each other
* The links found on a page is traversed recursivly and in parallell
* The pages found are written to disk to a directory structure corresponding to the url path, with `root` corresponding to the website root
* Any errors that occurs are aggregated up to the main program task, and displayed after the traversing is finished
