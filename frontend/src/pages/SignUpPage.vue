<script setup>
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const name = ref('');
const email = ref('');
const password = ref('');
const role = ref('Student');
const success = ref('');
const error = ref('');

const submit = async () => {
  try {
    await axios.post(`${import.meta.env.VITE_API_URL}/Auth/register`, {
      name: name.value,
      email: email.value,
      role: role.value,
      password: password.value
    });
    success.value = 'Cont creat cu succes!';
    setTimeout(() => {
      router.push('/login');
    }, 1000); // așteaptă 1 secundă pentru a vedea mesajul
    error.value = '';
    name.value = email.value = password.value = '';
  } catch (err) {
    error.value = err.response?.data || 'Eroare la creare cont.';
    success.value = '';
  }
};
</script>
<style src="../assets/signup.css"></style>
<template>
  <div class="signup-container">
    <h2>Creare cont</h2>
    <form @submit.prevent="submit">
      <input v-model="name" placeholder="Nume complet" required />
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Parolă" required />
      <select v-model="role">
        <option>Student</option>
        <option>Profesor</option>
      </select>
      <button type="submit">Înregistrează-te</button>
    </form>
    <p v-if="success" style="color:green">{{ success }}</p>
    <p v-if="error" style="color:red">{{ error }}</p>
    <p>Ai deja cont? <router-link to="/login">Autentifică-te aici</router-link></p>
  </div>
</template>



