import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from "axios";
import process from 'node:process';

axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "http://localhost:5000/api";
import './assets/main.css'
const app = createApp(App)
app.use(router)
app.mount('#app')
