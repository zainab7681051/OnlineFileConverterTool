import { createApp } from 'vue'
import App from './App.vue'
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
createApp(App).mount('#app')
const firebaseConfig = {
    apiKey: "AIzaSyCTrBFb1oinToeRzWdjcCTmrTmmXtqzRVg",
    authDomain: "fileconvertertool.firebaseapp.com",
    projectId: "fileconvertertool",
    storageBucket: "fileconvertertool.appspot.com",
    messagingSenderId: "757435787513",
    appId: "1:757435787513:web:f0c8594f89f97a3d425a28",
    measurementId: "G-9GQMWXM1HN"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);

