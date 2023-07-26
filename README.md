
# DeINF (release 2.0)
[![AGPLv3 License](https://img.shields.io/badge/License-GNU%20AGPL%20v3-brightgreen.svg)](https://opensource.org/license/agpl-v3/)

![Destination Home's logo](https://avatars.githubusercontent.com/u/88275131?size=64)

Open-source data management software, developed by the Destination Home team, to facilitate the storage and organization of donated cache data for PlayStation Home.




## Authors

- Zeph — [@ZephyrCodesStuff](https://www.github.com/ZephyrCodesStuff)
- Knight — [@I-Knight-I](https://www.github.com/I-Knight-I)


## Context
Here's some useful info to better explain the scope of this tool.

### What is an INF file?
An 'INF' file is basically 'INFormation' for a corresponding 'DAT' file (data). It can contain things such as the maximum file size, the URI, upload date of the DAT file on the CDN, etc. 

### Why does this tool exist?
Until now, cache data has always been difficult to manage, both due to its size (which, when fully "mapped" *en masse*, can easily exceed terabytes of storage space) and due to the fact that there has never really been a centraliszed and organized space to upload it.

Another big problem with it are duplicates. A lot of cache data has contents that other caches might contain as well. There was no easy way to fully automate a distinction process, so a lot of the data is actually duplicate.

**This software aims to reduce that as much as possible**, by keeping a central database of all the data currently stored, alongside all of the metadata it can gather from it. This allows to avoid uploading and storing duplicate data, retaining only what is unique.
## Backend
DeINF 2.0 requires its backend counterpart to function, as it's meant to serve as a centralized system, with multiple clients being able to access its data, in order to contribute to it.

While this client is fully open-source, the entire project is meant to serve as a centralized system; as such, **the backend will not be publicly available**.
## Compilation

This project is deeply integrated with Microsoft's new Windows UI Library 3, therefore requiring a fairly recent version of Windows to run.

To compile it, make sure you have installed the latest version of Visual Studio (*the purple one*), along with your Windows version's SDK.

Finally, clone the repo, open the solution and build it. Make sure to run the **packaged** version, as the unpackaged one *does not* contain the required UI libraries.

## Installation

We currently lack signed certificates to sign the code with; as such, you will have to add our self-signed certificate to the Windows certificate registry, under the "Trusted People" section.

You will have to create certificates to sign the package, should you build the project yourself.

Our MSIX packages and certificates are available on the [releases](https://www.github.com/DestinationHome/DeINF-2.0/releases) page.


## Feedback

If you have any suggestions, you are welcome to join Destination Home's [Discord server](https://discord.com/invite/QguSBT3) and tell us what you think should be improved.