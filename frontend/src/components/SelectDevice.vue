<template>
<div class="radio-input">
  <input id="r1" name="device" type="radio" class="input" value="1" @input="$emit('update:modelValue', $event.target.value)" checked="">
  <!-- <label for="r1">a</label> -->
  <input id="r2" name="device" type="radio" class="input" value="2" @input="$emit('update:modelValue', $event.target.value)">
  <!-- <label for="r2">b</label> -->
  <input id="r3" name="device" type="radio" class="input" value="3" @input="$emit('update:modelValue', $event.target.value)">
  <!-- <label for="r3">c</label> -->
  <input id="r4" name="device" type="radio" class="input" value="4" @input="$emit('update:modelValue', $event.target.value)">
  <!-- <label for="r4">d</label> -->
</div>
</template>

<script>
export default {
    data(){
        return{}
    },
    props: ['modelValue'],
}
</script>

<style scoped>
.input {
  -webkit-appearance: none;
 /* remove default */
  /* display: block; */
  margin: 10px;
  width: 24px;
  height: 24px;
  border-radius: 12px;
  cursor: pointer;
  vertical-align: middle;
  box-shadow: hsla(0,0%,100%,.15) 0 1px 1px, inset hsla(0,0%,0%,.5) 0 0 0 1px;
  background-color: hsla(0,0%,0%,.2);
  background-image: -webkit-radial-gradient( hsla(160, 100%, 37%, 1) 0%, hsla(160, 100%, 30%, 1) 15%, hsla(160, 100%, 30%, 0.3) 28%, hsla(160, 100%, 30%, 0) 70% );
  background-repeat: no-repeat;
  -webkit-transition: background-position .15s cubic-bezier(.8, 0, 1, 1),
    -webkit-transform .25s cubic-bezier(.8, 0, 1, 1);
  outline: none;
}

.input:checked {
  -webkit-transition: background-position .2s .15s cubic-bezier(0, 0, .2, 1),
    -webkit-transform .25s cubic-bezier(0, 0, .2, 1);
}

.input:active {
  -webkit-transform: scale(1.5);
  -webkit-transition: -webkit-transform .1s cubic-bezier(0, 0, .2, 1);
}

/* The up/down direction logic */

.input,
.input:active {
  background-position: 24px 0;
}

.input:checked {
  background-position: 0 0;
}

.input:checked ~ .input,
.input:checked ~ .input:active {
  background-position: -24px 0;
}


</style>