// src/main.js
import './assets/global.css'
import './assets/app.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router' // importă routerul

createApp(App).use(router).mount('#app')
