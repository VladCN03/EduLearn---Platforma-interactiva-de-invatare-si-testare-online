<template>
  <aside class="sidebar">
    <h2 class="logo">EduLearn</h2>
    <ul>
      <li v-for="page in filteredPages" :key="page.title">
        <router-link :to="page.route" class="nav-link">
          <i :class="`mdi ${page.icon}`"></i> {{ page.title }}
        </router-link>
      </li>
    </ul>

    <!-- Afișăm doar dacă utilizatorul e logat -->
    <div v-if="user" class="user-section">
      <div class="user-info">
        <strong>{{ user.name }}</strong>
        <span class="user-role">{{ user.role }}</span>
      </div>
      <button class="logout-btn" @click="$emit('logout')">Logout</button>
    </div>

  </aside>
</template>

<script>
import { useRouter } from 'vue-router';

export default {
  props: ['pages', 'user'],
  emits: ['logout'],
  data() {
    return {
      userMenuOpen: false
    };
  },
  computed: {
    filteredPages() {
      return this.pages.filter(p => !p.adminOnly || this.user?.role === 'Admin');
    }
  },
  setup(_, { emit }) {
    const router = useRouter();

    const logout = () => {
      localStorage.removeItem('token');
      emit('logout');         // Trimitem semnal către App.vue
      router.push('/login');  // Redirecționăm
    };

    return { logout };
  }
};
</script>

<style src="../assets/sidebar.css"></style>
