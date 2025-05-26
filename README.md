# ST10442835_PROG6221_Part2  
**Cybersecurity Awareness ChatBot - README**

## Overview
This chatbot is designed to raise awareness about cybersecurity by simulating interactive conversations. Built using C#, it helps users understand best practices in password safety, phishing prevention, privacy, and safe browsing through personalized, engaging dialogue.

## Part 2 Enhancements
- **Keyword Recognition:** Detects key phrases like "password safety", "phishing", "privacy", and "scam" to provide relevant cybersecurity advice.
- **Random Responses:** Offers varied, randomized answers to avoid repetition and feel more human-like.
- **Memory and Recall:** Remembers topics the user is interested in (e.g., “I’m interested in phishing”) and recalls them later upon request.
- **Sentiment Detection:** Responds to emotional cues like "worried", "mad", or "curious" to personalize tone and support.
- **Conversation Flow:** Tracks last discussed topics and handles follow-up requests like “tell me more” or “remind me”.
- **Delegates:** Demonstrates use of delegates for handling response output.

## Features
- **Interactive Chat Interface**: Real-time Q&A with personalized feedback.
- **Color-coded Console Output**: Enhanced readability for inputs and responses.
- **Typewriter Effect**: Bot responses are displayed with a smooth animation.
- **Startup Sound + ASCII Logo**: Welcomes the user with a logo and sound effect.
- **Error Handling**: Graceful input validation and robust exception handling.

## Topics Covered
- Password safety
- Phishing prevention
- Scam awareness
- Privacy tips
- General safe browsing practices

## Components
ChatBot.cs - Main chatbot logic and response handling
Display.cs - Handles all console output and user input
Voice.cs - Manages audio playback for welcome sound
Program.cs - Entry point with error handling
Logo.txt - ASCII art logo displayed at startup
ChatBotRecording.wav - Optional sound file for startup

## How to Use
Run the program

Enter your name when prompted

Ask questions about:

Password safety ("How to create strong passwords?")

Phishing ("What is phishing?")

Safe browsing ("How to browse safely?")

Type "exit" or "quit" to end the session

Requirements
.NET 9.0

Windows OS (for sound playback functionality)

Logo.txt file in the executable directory
