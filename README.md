# Server
I worked on a project that involved developing a client-server chat application using C# and Windows Forms. The project included the following functionalities:

1. Server Application:
   - Implemented a server application using TCP/IP sockets to handle multiple client connections.
   - Utilized multithreading to handle client connections concurrently.
   - Advertised the server's presence to clients using UDP broadcasting.
   - Supported sending and receiving text messages and image files between clients.

2. Client Application:
   - Developed a client application with a graphical user interface using Windows Forms.
   - Established a connection with the server using TCP/IP sockets.
   - Displayed incoming text messages and image files from other clients.
   - Sent text messages and image files to other clients via the server.

3. Text Messaging:
   - Allowed clients to send and receive text messages in real-time.
   - Maintained a list of connected clients on the server.
   - Handled disconnections and removal of clients from the list.

4. Image File Transfer:
   - Supported sending and receiving image files between clients.
   - Enabled clients to select an image file from their local system and send it to a specific recipient.
   - Implemented the transmission and reception of image files using TCP/IP sockets.
   - Displayed the received images on the client's interface.

The project involved socket programming, multithreading, graphical user interface design, and networking concepts. It provided a chat application that allowed clients to communicate with each other using text messages and exchange image files.
