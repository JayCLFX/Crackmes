# 🔒 Crackme

This repository contains a collection of C# authentication challenges (Crackmes) developed for security research and educational purposes. The goal of these challenges is to understand how authentication logic, hashing, and environmental dependencies work in a .NET environment.

## 📂 Project Overview
Each project in this folder is designed to test different aspects of binary security:
- **Challenge 01:** Explores the use of environment variables, Hashing (SHA256), and state-machine logic.
- **Future Challenges:** [Pending updates with more complex logic...]

## 🛠 Technical Concepts
These challenges utilize the following security-related concepts:
- **Cryptography:** Implementation of `SHA256` and `RNGCryptoServiceProvider`.
- **System Integrity:** Using `Environment` variables and process states.
- **Logical Flow:** Implementing checkpoints to prevent simple code-patching.



## ⚠️ Legal & Ethical Disclaimer
These programs are for **educational purposes only**. The goal is to learn about software protection and reverse engineering. Do not use these techniques for malicious purposes or on unauthorized software.

---

## 🎯 How to Participate
If you are analyzing these challenges, consider investigating:
1. **The Entry Point:** How is the `Main` method initialized?
2. **The Validation Loop:** Can you bypass the `Check()` method by modifying the application state in memory?
3. **Environment Dependencies:** How do the environmental variables influence the outcome?

## 🛡️ Best Practices
*Never share sensitive keys or credentials in production environments.* *This repository is intended to teach how these mechanisms can be implemented—and how easily they can be bypassed if not architected securely.*
