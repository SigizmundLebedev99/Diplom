<template>
    <v-layout column class="mt-2">
        <v-toolbar color="transparent" flat dense>
            <v-toolbar-title>Единицы работы</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon @click="preWICreating()">
                <v-icon>
                    add
                </v-icon>
            </v-btn>
            <v-btn icon>
                <v-icon>
                    search
                </v-icon>
            </v-btn>
        </v-toolbar>
    </v-layout>
</template>

<script>
import {mapActions} from 'vuex'
export default {
    methods:{
        ...mapActions({preWICreating:'createWorkItem/preWICreating'})
    }
}
</script>

