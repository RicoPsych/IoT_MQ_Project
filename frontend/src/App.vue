<script setup>

import { RouterLink, RouterView } from 'vue-router'

import Navigation  from './components/Navigation.vue'

</script>

<template>
  <Navigation/>
  <RouterView/>
</template>

<style>
* {
  padding: 0;
  margin: 0;
  box-sizing: border-box;
  /* include the padding and border in an element's total width */
}
</style>
