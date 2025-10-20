import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '../App.vue'
import LoginPage from '../pages/LoginPage.vue'
import SignupPage from '../pages/SignupPage.vue'
import StatisticsPage from "@/pages/StatisticsPage.vue";
import LessonsPage from "@/pages/LessonsPage.vue";
import HomePage from "@/pages/HomePage.vue";
import TestsPage from "@/pages/TestsPage.vue";

const routes = [

    { path: '/', component: HomePage, meta: { requiresAuth: true } },
    { path: '/lessons', component: LessonsPage, meta: { requiresAuth: true } },
    { path: '/tests', component: TestsPage, meta: { requiresAuth: true } },
    { path: '/statistics', component: StatisticsPage, meta: { requiresAuth: true } },
    {path: '/login', component: LoginPage},
    {path: '/signup', component: SignupPage}
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {
    const isAuthenticated = !!localStorage.getItem('token');

    if (to.meta.requiresAuth && !isAuthenticated) {
        next('/login'); // Redirect către login dacă nu e logat
    } else {
        next(); // Permite navigarea
    }
});


export default router
