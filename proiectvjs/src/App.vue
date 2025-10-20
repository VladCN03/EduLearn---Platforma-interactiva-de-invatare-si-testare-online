<template>
  <div class="app-layout">
    <Sidebar :pages="pages" :user="user" @logout="handleLogout" />
    <div class="main-content">
      <router-view :user="user" @login-success="handleLogin" />
    </div>
  </div>
</template>

<!--<script>-->
<!--import Sidebar from './components/Sidebar.vue'-->

<!--export default {-->
<!--  components: { Sidebar },-->
<!--  data() {-->
<!--    return {-->
<!--      user: { name: "Maria Popescu", role: "Student" }-->
<!--    }-->
<!--  }-->
<!--}-->
<!--</script>-->

<script>
import Sidebar from './components/Sidebar.vue'
import HomePage from './pages/HomePage.vue'
import LessonsPage from './pages/LessonsPage.vue'
import TestsPage from './pages/TestsPage.vue'
// Dacă vrei să adaugi StatisticsPage:
import StatisticsPage from './pages/StatisticsPage.vue'

export default {
  components: {
    Sidebar,
    HomePage,
    LessonsPage,
    TestsPage,
    StatisticsPage, // adaugă aici dacă vrei și pagina de statistici
  },
  data() {
    return {
      user: null,
      pages: [
        { title: "Home", icon: "mdi-view-dashboard", route: "/" },
        { title: "Lessons", icon: "mdi-file-document", route: "/lessons" },
        { title: "Tests", icon: "mdi-clipboard-check", route: "/tests" },
        { title: "Statistics", icon: "mdi-chart-bar", route: "/statistics" }
      ],
      activePage: { component: 'HomePage' },
    }
  },
  created() {
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      try {
        this.user = JSON.parse(savedUser);
      } catch (err) {
        console.error('Eroare la parsare user:', err);
        this.user = null;
        localStorage.removeItem('user');
      }
    }
  },
  methods: {
    handleLogin(userData) {
      this.user = userData;
    },
    handleLogout() {
      this.user = null;
      localStorage.removeItem('user');
      localStorage.removeItem("token");
      this.$router.push('/login');
    }
  }
}
</script>

<style scoped>
</style>
