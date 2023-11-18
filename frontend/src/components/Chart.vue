<template>
  <div class="backdrop" @click.self="closeAddHabit">
    <div class="container">
      <Line id="my-chart-id" :options="chartOptions" :data="chartData" />
    </div>
  </div>
</template>

<script>
import { Line } from "vue-chartjs";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export default {
  components: { Line },
  data() {
    return {
      chartData: {
        labels: this.dates,
        datasets: [
          {
            backgroundColor: "hsla(160, 100%, 37%, 1)",
            borderColor: "hsla(160, 100%, 17%, 1)",
            data: this.values,
            label: this.title,
          },
        ],
      },
      chartOptions: {
        responsive: true,
      },
    };
  },
  props: {
    values: null,
    dates: null,
    title: null,
  },
  methods: {
    closeAddHabit() {
      this.$emit("closeChart");
    },
  },
  created(){

  }
};
</script>

<style scoped>
.container {
  width: 80%;
  height: 80%;
  padding: 50px;
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  flex-direction: column;
  border: 1px solid hsla(160, 100%, 17%, 1);
  background: var(--color-background);
}

.backdrop {
  top: 0;
  left: 0;
  position: fixed;
  background: rgba(0, 0, 0, 0.65);
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99;
}
</style>