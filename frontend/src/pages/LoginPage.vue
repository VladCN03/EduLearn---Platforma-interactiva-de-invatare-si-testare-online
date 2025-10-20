<script setup>
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const email = ref('');
const password = ref('');
const error = ref('');
const router = useRouter();

// ✅ Adaugă linia aceasta
const emit = defineEmits(['login-success']);

const login = async () => {
  try {
    const res = await axios.post(`${import.meta.env.VITE_API_URL}/Auth/login`, {
      email: email.value,
      password: password.value
    });

    localStorage.setItem('token', res.data.token);
    localStorage.setItem('user', JSON.stringify(res.data.user));

    emit('login-success', res.data.user);  // ✅ Acum funcționează corect

    router.push('/');
  } catch (err) {
    error.value = 'Email sau parolă greșite.';
  }
}
</script>
<style src="../assets/login.css"></style>

<template>
  <div class="auth-container">
    <h2>Autentificare</h2>
    <form @submit.prevent="login">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Parolă" required />
      <button type="submit">Autentificare</button>
    </form>
    <p v-if="error" style="color:red">{{ error }}</p>

    <p>Nu ai cont? <router-link to="/signup">Înregistrează-te aici</router-link></p>
  </div>
</template>

